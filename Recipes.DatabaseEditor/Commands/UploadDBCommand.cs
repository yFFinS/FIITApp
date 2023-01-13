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
        DataBase.Upload("Products");

        _output.WriteLine("Products uploaded");

        DataBase.Upload("Recipes");

        _output.WriteLine("Recipes uploaded");
    }
}