namespace Recipes.DatabaseEditor.Commands;

public class DownloadProductsDBCommand : Command
{
    private readonly TextWriter _output;

    public DownloadProductsDBCommand(TextWriter output)
        : base(new[] { "download", "products" },
            "Скачать базу данных продуктов - download products")
    {
        _output = output;
    }

    public override void Execute(string[] args)
    {
        DataBase.Download("Products");

        _output.WriteLine("Products downloaded");
    }
}