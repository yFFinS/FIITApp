using System.Net;

namespace Recipes.DatabaseEditor;

public static class DataBase
{
    public static void Upload(string item)
    {
        var info = File.ReadAllLines("DataBaseAccess.txt");

        var ip = "ftp://" + info[0] + "/" + item + ".xml";
        var UserId = info[1];
        var Password = info[2];

        var path = GetDirectoryOfDBs() + item + ".xml";

        using WebClient client = new();
        client.Credentials = new NetworkCredential(UserId, Password);
        client.UploadFile(ip, WebRequestMethods.Ftp.UploadFile, path);
    }

    public static void Download(string item)
    {
        var info = File.ReadAllLines("DataBaseAccess.txt");

        var ip = "ftp://" + info[0] + "/" + item + ".xml";
        var UserId = info[1];
        var Password = info[2];

        var path = GetDirectoryOfDBs() + item + ".xml";

        using WebClient client = new();
        client.Credentials = new NetworkCredential(UserId, Password);
        client.DownloadFile(ip, path);
    }

    private static string GetDirectoryOfDBs()
    {
        var slash = Path.GetFullPath("/")[2..];
        var rootPath = Environment.CurrentDirectory.Split(slash);
        while (rootPath[rootPath.Length - 1] != "FIITApp")
        {
            rootPath = rootPath[..(rootPath.Length - 2)];
        }
        return string.Join(slash, rootPath) + slash + "Recipes.Presentation.Desktop" + slash;
    }
}

