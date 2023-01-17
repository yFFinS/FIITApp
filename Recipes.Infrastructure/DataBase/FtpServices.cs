using Microsoft.Extensions.Logging;
using System.Net;

namespace Recipes.Infrastructure.DataBase;

public record FtpDatabaseDetails(string RemotePath, string UserId, string Password, string DatabasePath);

public class FtpServices
{
    private readonly ILogger<FtpServices> _logger;
    private readonly DatabasePathsProvider _pathsProvider;

    public FtpServices(ILogger<FtpServices> logger, DatabasePathsProvider pathsProvider)
    {
        _logger = logger;
        _pathsProvider = pathsProvider;
    }

    private string GetLastModifiedFilePath(DatabaseName databaseName)
    {
        var appPath = _pathsProvider.GetAppPath();
        return Path.Join(appPath, databaseName + "LastModified.txt");
    }

    public void DownloadUpdatesIfPresent()
    {
        DownloadUpdatesIfPresent(DatabaseAccess.User, DatabaseName.Products);
        DownloadUpdatesIfPresent(DatabaseAccess.User, DatabaseName.Recipes);
    }

    public void DownloadUpdatesIfPresent(DatabaseAccess databaseAccess, DatabaseName databaseName)
    {
        _logger.LogInformation("Checking for updates for {DatabaseName} as {DatabaseAccess}", databaseName, databaseAccess);

        var details = GetDatabaseDetails(databaseAccess, databaseName);

        var ftpRequest = (FtpWebRequest)WebRequest.Create(details.RemotePath);
        ftpRequest.Credentials = new NetworkCredential(details.UserId, details.Password);
        ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
        var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        var newDateLastModified = ftpResponse.LastModified.Ticks;

        if (!File.Exists(details.DatabasePath))
        {
            DownloadModified(databaseAccess, databaseName, newDateLastModified);
            return;
        }

        var lastModifiedRaw = File.ReadAllLines(GetLastModifiedFilePath(databaseName))[0];
        var parsed = long.TryParse(lastModifiedRaw, out var dateLastModified);
        if (!parsed || dateLastModified < newDateLastModified)
        {
            DownloadModified(databaseAccess, databaseName, newDateLastModified);
        }
    }

    private void DownloadModified(DatabaseAccess databaseAccess, DatabaseName databaseName, long newDateLastModified)
    {
        Download(databaseAccess, databaseName);
        var lastModifiedFilePath = GetLastModifiedFilePath(databaseName);
        File.WriteAllText(lastModifiedFilePath, newDateLastModified.ToString());
    }

    public void Upload(DatabaseAccess databaseAccess, DatabaseName databaseName)
    {
        _logger.LogInformation("Uploading {DatabaseName} as {DatabaseAccess}", databaseName, databaseAccess);

        var details = GetDatabaseDetails(databaseAccess, databaseName);

        using var client = new WebClient();
        client.Credentials = new NetworkCredential(details.UserId, details.Password);
        client.UploadFile(details.RemotePath, details.DatabasePath);
    }

    public void Download(DatabaseAccess databaseAccess, DatabaseName databaseName)
    {
        _logger.LogInformation("Downloading {DatabaseName} as {DatabaseAccess}", databaseName, databaseAccess);

        var details = GetDatabaseDetails(databaseAccess, databaseName);

        using var client = new WebClient();
        client.Credentials = new NetworkCredential(details.UserId, details.Password);
        client.DownloadFile(details.RemotePath, details.DatabasePath);
    }

    private static string GetCredentialsPath(DatabaseAccess databaseAccess)
    {
        return databaseAccess switch
        {
            DatabaseAccess.User => "UserAccess.txt",
            DatabaseAccess.Admin => "AdminAccess.txt",
            _ => throw new ArgumentOutOfRangeException(nameof(databaseAccess), databaseAccess, null)
        };
    }

    private FtpDatabaseDetails GetDatabaseDetails(DatabaseAccess databaseAccess, DatabaseName databaseName)
    {
        var info = File.ReadAllLines(GetCredentialsPath(databaseAccess));
        var databasePath = _pathsProvider.GetDatabasePath(databaseName);

        var remoteDbPath = "ftp://" + info[0] + "/" + Path.GetFileName(databasePath);
        var userId = info[1];
        var password = info[2];

        return new FtpDatabaseDetails(remoteDbPath, userId, password, databasePath);
    }
}