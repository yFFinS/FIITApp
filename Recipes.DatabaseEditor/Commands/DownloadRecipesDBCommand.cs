namespace Recipes.DatabaseEditor.Commands;

public class DownloadRecipesDBCommand : Command
{
    private readonly TextWriter _output;

    public DownloadRecipesDBCommand(TextWriter output)
        : base(new[] { "download", "recipes" },
            "Скачать базу данных рецептов - download recipes")
    {
        _output = output;
    }

    public override void Execute(string[] args)
    {
        DataBase.Download("Recipes");

        _output.WriteLine("Recipes downloaded");
    }
}