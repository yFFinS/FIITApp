namespace Recipes.DatabaseEditor.Commands;

public class DownloadDBCommand : Command
{
    private readonly TextWriter _output;

    public DownloadDBCommand(TextWriter output)
        : base(new[] { "download", "db" },
            "Скачать базу данных - download db")
    {
        _output = output;
    }

    public override void Execute(string[] args)
    {
        DataBase.Download("Products");

        _output.WriteLine("Products downloaded");

        DataBase.Download("Recipes");

        _output.WriteLine("Recipes downloaded");
    }
}