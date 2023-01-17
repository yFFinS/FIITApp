using Recipes.Infrastructure;

namespace Recipes.DatabaseEditor.Commands;

public class UploadDBCommand : Command
{
    private readonly TextWriter _output;

    public UploadDBCommand(TextWriter output)
        : base(new[] { "upload", "db" },
            "Загрузить базу данных - upload db")
    {
        _output = output;
    }

    public override void Execute(string[] args)
    {
        FTPServices.Upload("AdminAccess", "Products");
        _output.WriteLine("Products uploaded");

        FTPServices.Upload("AdminAccess", "Recipes");
        _output.WriteLine("Recipes uploaded");
    }
}