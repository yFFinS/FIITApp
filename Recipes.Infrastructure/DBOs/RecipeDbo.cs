namespace Recipes.Infrastructure;

public class RecipeDbo : EntityDbo<RecipeDbo>
{
    public string Title { get; set; }
    public int Servings { get; set; }
    public TimeSpan CookDuration { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public IngredientDbo[] IngredientDbos { get; set; }
    public CookingTechniqueDbo CookingTechniqueDbo { get; set; }
}