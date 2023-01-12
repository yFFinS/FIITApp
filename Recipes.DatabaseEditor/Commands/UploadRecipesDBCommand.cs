namespace Recipes.DatabaseEditor.Commands;

public class UploadRecipesDBCommand : Command
{
    private readonly TextWriter _output;

    public UploadRecipesDBCommand(TextWriter output)
        : base(new[] { "upload", "recipes" },
            "Загрузить базу данных рецептов - upload recipes")
    {
        _output = output;
    }

    public override void Execute(string[] args)
    {
        DataBase.Upload("Recipes");

        _output.WriteLine("Recipes uploaded");
    }
}