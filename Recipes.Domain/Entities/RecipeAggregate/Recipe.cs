using Recipes.Domain.Entities.CookingTechnicAggregate;
using Recipes.Domain.Entities.IngredientsAggregate;

namespace Recipes.Domain.Entities.RecipeAggregate;

public class Recipe
{
    public Ingredients Ingredients { get; }
    public CookingTechnic CookingTechnic { get; }

    public Recipe(Ingredients ingredients, CookingTechnic cookingTechnic)
    {
        Ingredients = ingredients;
        CookingTechnic = cookingTechnic;
    }
}