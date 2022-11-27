using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.DatabaseEditor.Commands;

public class RemoveRecipeCommand : Command
{
    private readonly TextWriter _output;
    private readonly IRecipeRepository _recipeRepository;

    public RemoveRecipeCommand(TextWriter output, IRecipeRepository recipeRepository)
        : base(new[] { "remove", "recipe" },
            "Удалить рецепт из базы данных - remove recipe <id or recipe name>")
    {
        _output = output;
        _recipeRepository = recipeRepository;
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
            var recipe = _recipeRepository.GetRecipeByNameAsync(idOrName).Result;
            if (recipe is null)
            {
                _output.WriteLine($"Рецепт с именем {idOrName} не найден");
                return;
            }

            entityId = recipe.Id;
        }

        _recipeRepository.RemoveRecipesByIdAsync(new[] { entityId }).Wait();

        _output.WriteLine(isId ? $"Рецепт с id {id} удален" : $"Рецепт с именем {idOrName} удален");
    }
}