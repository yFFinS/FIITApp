using Microsoft.Extensions.Logging;
using System.Net;

namespace Recipes.Infrastructure.DataBase;

public record FtpDatabaseDetails(string RemotePath, string UserId, string Password);

public class FtpService
{
    private static readonly TimeSpan ImageDownloadInterval = TimeSpan.FromSeconds(0.2);
    private DateTime _lastImageDownload = DateTime.MinValue;

    private readonly ILogger<FtpService> _logger;
    private readonly DatabasePathsProvider _pathsProvider;

    public FtpService(ILogger<FtpService> logger, DatabasePathsProvider pathsProvider)
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
        _logger.LogInformation("Checking for updates for {DatabaseName} as {DatabaseAccess}", databaseName,
            databaseAccess);

        var details = GetDatabaseDetails(databaseAccess);
        var remotePath = GetRemotePath(details.RemotePath, databaseName);
        var databasePath = _pathsProvider.GetDatabasePath(databaseName);

        var ftpRequest = (FtpWebRequest)WebRequest.Create(remotePath);
        ftpRequest.Credentials = new NetworkCredential(details.UserId, details.Password);
        ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
        var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        var newDateLastModified = ftpResponse.LastModified.Ticks;

        if (!File.Exists(databasePath))
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

        var details = GetDatabaseDetails(databaseAccess);
        var remotePath = GetRemotePath(details.RemotePath, databaseName);
        var databasePath = Path.GetFileName(_pathsProvider.GetDatabasePath(databaseName));

        using var client = new WebClient();
        client.Credentials = new NetworkCredential(details.UserId, details.Password);
        client.UploadFile(remotePath, databasePath);
    }


    public void Download(DatabaseAccess databaseAccess, DatabaseName databaseName)
    {
        _logger.LogInformation("Downloading {DatabaseName} as {DatabaseAccess}", databaseName, databaseAccess);

        var details = GetDatabaseDetails(databaseAccess);
        var remotePath = GetRemotePath(details.RemotePath, databaseName);
        var databasePath = _pathsProvider.GetDatabasePath(databaseName);

        using var client = new WebClient();
        client.Credentials = new NetworkCredential(details.UserId, details.Password);
        client.DownloadFile(remotePath, databasePath);
    }

    private string GetRemotePath(string serverRemotePath, DatabaseName databaseName)
    {
        return serverRemotePath + Path.GetFileName(_pathsProvider.GetDatabasePath(databaseName));
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

    private FtpDatabaseDetails GetDatabaseDetails(DatabaseAccess databaseAccess)
    {
        var info = File.ReadAllLines(GetCredentialsPath(databaseAccess));

        var remoteDbPath = "ftp://" + info[0] + "/";
        var userId = info[1];
        var password = info[2];

        return new FtpDatabaseDetails(remoteDbPath, userId, password);
    }

    private async Task<MemoryStream> DownloadImageWithIntervalAsync(Uri imageUri)
    {
        while (DateTime.Now - _lastImageDownload < ImageDownloadInterval)
        {
            await Task.Delay(15);
        }

        try
        {
            return await WebStreamLoader.LoadAsync(imageUri);
        }
        finally
        {
            _lastImageDownload = DateTime.Now;
        }
    }

    public async Task UploadImage(Uri imageUri)
    {
        var details = GetDatabaseDetails(DatabaseAccess.Admin);
        var uploadPath = GetRemoteImagePath(imageUri, details);

        byte[] buffer;
        await using (var stream = await DownloadImageWithIntervalAsync(imageUri))
        {
            buffer = new byte[stream.Length];
            _ = await stream.ReadAsync(buffer);
        }

        using var client = new WebClient();
        client.Credentials = new NetworkCredential(details.UserId, details.Password);
        client.UploadData(uploadPath, buffer);
    }

    public async Task<byte[]> DownloadImage(Uri imageUri)
    {
        var details = GetDatabaseDetails(DatabaseAccess.User);
        var downloadPath = GetRemoteImagePath(imageUri, details);

        using var client = new WebClient();
        client.Credentials = new NetworkCredential(details.UserId, details.Password);
        return await client.DownloadDataTaskAsync(downloadPath);
    }

    private static string GetRemoteImagePath(Uri imageUri, FtpDatabaseDetails details)
    {
        var safeUri = GetSafeUri(imageUri);
        return details.RemotePath + "Images/" + safeUri;
    }

    private static string GetSafeUri(Uri imageUri)
    {
        var safeUri = imageUri.ToString().Replace(":", "_").Replace("/", "_");
        // Replace all dots with underscores, except the last one
        var lastDotIndex = safeUri.LastIndexOf('.');
        safeUri = safeUri[..lastDotIndex].Replace(".", "_") + safeUri[lastDotIndex..];
        return safeUri;
    }
}