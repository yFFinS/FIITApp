using System.Net;
using System.Runtime.CompilerServices;

namespace Recipes.Infrastructure;

public static class FTPServices
{
    public static void CheckForUpdates(string dbAccessFileName, string dbName)
    {
        PrepareToConnect(
            dbAccessFileName, dbName,
            out string remoteDBPath, out string UserId, out string Password,
            out string dbPath);

        var ftpRequest = (FtpWebRequest)WebRequest.Create(remoteDBPath);
        ftpRequest.Credentials = new NetworkCredential(UserId, Password);
        ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
        var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        var newDateLastModified = ftpResponse.LastModified.ToString();

        var appPath = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RecipeKeeper");

        var lastModifiedFilePath = Path.Join(appPath, dbName + "LastModified.txt");

        if (!File.Exists(dbPath))
        {
            Download(dbAccessFileName, dbName);
            File.WriteAllText(lastModifiedFilePath, newDateLastModified);
        }
        else
        {
            try
            {
                var dateLastModified = File.ReadAllLines(lastModifiedFilePath)[0];
                if (!dateLastModified.Equals(newDateLastModified))
                {
                    Download(dbAccessFileName, dbName);
                    File.WriteAllText(lastModifiedFilePath, newDateLastModified);
                }
            }
            catch
            {
                Download(dbAccessFileName, dbName);
                File.WriteAllText(lastModifiedFilePath, newDateLastModified);
            }
        }
    }

    public static void Upload(string dbAccessFileName, string dbName)
    {
        PrepareToConnect(
            dbAccessFileName, dbName,
            out string remoteDBPath, out string UserId, out string Password,
            out string dbPath);

        using WebClient client = new();
        client.Credentials = new NetworkCredential(UserId, Password);
        client.UploadFile(remoteDBPath, WebRequestMethods.Ftp.UploadFile, dbPath);
    }

    public static void Download(string dbAccessFileName, string dbName)
    {
        PrepareToConnect(
            dbAccessFileName, dbName,
            out string remoteDBPath, out string UserId, out string Password,
            out string dbPath);

        using WebClient client = new();
        client.Credentials = new NetworkCredential(UserId, Password);
        client.DownloadFile(remoteDBPath, dbPath);
    }

    private static void PrepareToConnect(
        string dbAccessFileName, string dbName,
        out string remoteDBPath, out string UserId, out string Password,
        out string dbPath)
    {
        var info = File.ReadAllLines(dbAccessFileName + ".txt");

        remoteDBPath = "ftp://" + info[0] + "/" + dbName + ".xml";
        UserId = info[1];
        Password = info[2];
        var appPath = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "RecipeKeeper");
        Directory.CreateDirectory(appPath);
        dbPath = Path.Join(
            appPath,
            dbName + ".xml");
    }

    public static void GetDBPaths(out string productsPath, out string recipesPath)
    {
        var appPath = Path.Join(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "RecipeKeeper");

        Directory.CreateDirectory(appPath);

        productsPath = Path.Join(appPath, "Products.xml");
        recipesPath = Path.Join(appPath, "Recipes.xml");
    }
}
