namespace Recipes.Infrastructure;

public record RecipeDbo(string Id, string Title, int Servings,
    TimeSpan CookDuration, string? Description, string? ImageUrl,
    IngredientDbo[] IngredientDbos, CookingTechniqueDbo CookingTechniqueDbo);