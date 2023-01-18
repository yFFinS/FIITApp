using Recipes.Infrastructure.DataBase;

namespace Recipes.DatabaseEditor.Commands;

public class UploadDbCommand : Command
{
    private readonly TextWriter _output;
    private readonly FtpService _ftpService;

    public UploadDbCommand(TextWriter output, FtpService ftpService)
        : base(new[] { "upload", "db" },
            "Загрузить базу данных - upload db")
    {
        _output = output;
        _ftpService = ftpService;
    }

    public override void Execute(string[] args)
    {
        _ftpService.Upload(DatabaseAccess.Admin, DatabaseName.Products);
        _output.WriteLine("Products uploaded");

        _ftpService.Upload(DatabaseAccess.Admin, DatabaseName.Recipes);
        _output.WriteLine("Recipes uploaded");
    }
}