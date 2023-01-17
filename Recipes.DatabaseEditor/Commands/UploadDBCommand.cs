using Recipes.Infrastructure.DataBase;

namespace Recipes.DatabaseEditor.Commands;

public class UploadDbCommand : Command
{
    private readonly TextWriter _output;
    private readonly FtpServices _ftpServices;

    public UploadDbCommand(TextWriter output, FtpServices ftpServices)
        : base(new[] { "upload", "db" },
            "Загрузить базу данных - upload db")
    {
        _output = output;
        _ftpServices = ftpServices;
    }

    public override void Execute(string[] args)
    {
        _ftpServices.Upload(DatabaseAccess.Admin, DatabaseName.Products);
        _output.WriteLine("Products uploaded");

        _ftpServices.Upload(DatabaseAccess.Admin, DatabaseName.Recipes);
        _output.WriteLine("Recipes uploaded");
    }
}