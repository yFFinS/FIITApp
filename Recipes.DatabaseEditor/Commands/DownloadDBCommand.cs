using Recipes.Infrastructure.DataBase;

namespace Recipes.DatabaseEditor.Commands;

public class DownloadDbCommand : Command
{
    private readonly TextWriter _output;
    private readonly FtpService _ftpService;

    public DownloadDbCommand(TextWriter output, FtpService ftpService)
        : base(new[] { "download", "db" },
            "Скачать базу данных - download db")
    {
        _output = output;
        _ftpService = ftpService;
    }

    public override void Execute(string[] args)
    {
        _ftpService.Download(DatabaseAccess.Admin, DatabaseName.Products);
        _output.WriteLine("Products downloaded");

        _ftpService.Download(DatabaseAccess.Admin, DatabaseName.Recipes);
        _output.WriteLine("Recipes downloaded");
    }
}