namespace Recipes.DatabaseEditor.Commands;

public class UploadProductsDBCommand : Command
{
    private readonly TextWriter _output;

    public UploadProductsDBCommand(TextWriter output)
        : base(new[] { "upload", "products" },
            "Загрузить базу данных продуктов - upload products")
    {
        _output = output;
    }

    public override void Execute(string[] args)
    {
        DataBase.Upload("Products");

        _output.WriteLine("Products uploaded");
    }
}