using Recipes.Application.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.DatabaseEditor.Commands;

public class RemoveProductsCommand : Command
{
    private readonly TextWriter _output;
    private readonly IProductRepository _productRepository;

    public RemoveProductsCommand(TextWriter output, IProductRepository productRepository)
        : base(new[] { "remove", "product" },
            "Удалить продукт из базы данных - remove product <id or product name>")
    {
        _output = output;
        _productRepository = productRepository;
    }

    public override void Execute(string[] args)
    {
        if (args.Length != 1)
        {
            _output.WriteLine("Неверное количество аргументов");
            return;
        }

        var idOrName = args[0];
        var isId = Guid.TryParse(idOrName, out var id);

        EntityId entityId;
        if (isId)
        {
            entityId = new EntityId(id);
        }
        else
        {
            var product = _productRepository.GetProductByNameAsync(idOrName).Result;
            if (product is null)
            {
                _output.WriteLine($"Продукт с именем {idOrName} не найден");
                return;
            }

            entityId = product.Id;
        }

        _productRepository.RemoveProductsByIdAsync(new[] { entityId }).Wait();

        _output.WriteLine(isId ? $"Продукт с id {id} удален" : $"Продукт с именем {idOrName} удален");
    }
}