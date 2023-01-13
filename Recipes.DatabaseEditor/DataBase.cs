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

        var path =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            + Path.GetFullPath("/")[2..]
            + item + ".xml";

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

        var path =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
            + Path.GetFullPath("/")[2..]
            + item + ".xml";

        using WebClient client = new();
        client.Credentials = new NetworkCredential(UserId, Password);
        client.DownloadFile(ip, path);
    }
}

