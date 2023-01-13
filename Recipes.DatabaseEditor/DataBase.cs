using System.Net;

namespace Recipes.DatabaseEditor;

public static class DataBase
{
    public static void Upload(string item)
    {
        string ip, UserId, Password, path;
        PrepareToConnect(item, out ip, out UserId, out Password, out path);

        using WebClient client = new();
        client.Credentials = new NetworkCredential(UserId, Password);
        client.UploadFile(ip, WebRequestMethods.Ftp.UploadFile, path);
    }

    public static void Download(string item)
    {
        string ip, UserId, Password, path;
        PrepareToConnect(item, out ip, out UserId, out Password, out path);

        using WebClient client = new();
        client.Credentials = new NetworkCredential(UserId, Password);
        client.DownloadFile(ip, path);
    }

    private static void PrepareToConnect(string item, out string ip, out string UserId, out string Password, out string path)
    {
        var info = File.ReadAllLines("DataBaseAccess.txt");

        ip = "ftp://" + info[0] + "/" + item + ".xml";
        UserId = info[1];
        Password = info[2];
        path = Path.Join(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            item + ".xml");
    }
}

