using Recipes.Infrastructure;
using Recipes.Infrastructure.DataBase;

namespace Recipes.DatabaseEditor.Commands;

public class DownloadDbCommand : Command
{
    private readonly TextWriter _output;
    private readonly FtpServices _ftpServices;

    public DownloadDbCommand(TextWriter output, FtpServices ftpServices)
        : base(new[] { "download", "db" },
            "Скачать базу данных - download db")
    {
        _output = output;
        _ftpServices = ftpServices;
    }

    public override void Execute(string[] args)
    {
        _ftpServices.Download(DatabaseAccess.Admin, DatabaseName.Products);
        _output.WriteLine("Products downloaded");

        _ftpServices.Download(DatabaseAccess.Admin, DatabaseName.Recipes);
        _output.WriteLine("Recipes downloaded");
    }
}