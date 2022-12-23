using System.ComponentModel;
using System.Xml.Serialization;
using Ardalis.GuardClauses;
using Recipes.Domain.Base;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.ValueObjects;

namespace Recipes.Infrastructure;

[Serializable]
[XmlType(TypeName = "Recipe")]
public class RecipeDataBaseObject : Entity<EntityId>
{
    private string _title = null!;
    private string? _description;
    private int _servings;
    private TimeSpan _cookingTime;
    private EnergyValue _energyValue = null!;

    public string Title
    {
        get => _title;
        set => _title = Guard.Against.Null(value);
    }

    public string? Description
    {
        get => _description;
        set => _description = value;
    }

    public int Servings
    {
        get => _servings;
        set => _servings = Guard.Against.NegativeOrZero(value);
    }

    public TimeSpan CookDuration
    {
        get => _cookingTime;
        set => _cookingTime = Guard.Against.NegativeOrZero(value);
    }

    public EnergyValue EnergyValue
    {
        get => _energyValue;
        set => _energyValue = Guard.Against.Null(value);
    }

    [XmlIgnore]
    public Uri? ImageUrl { get; set; }

    [XmlAttribute("uri")]
    [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
    public string? ImageUrlString
    {
        get { return ImageUrl?.ToString(); }
        set { ImageUrl = value == null ? null : new Uri(value); }
    }

    private RecipeDataBaseObject()
    {
    }

    private RecipeDataBaseObject(Recipe recipe) : base(recipe.Id)
    {
        Title = recipe.Title;
        EnergyValue = recipe.EnergyValue;
        _ingredientGroup = new IngredientGroup(recipe.Ingredients);
        _cookingTechnic = new CookingTechnic(recipe.CookingSteps);
        Description = recipe.Description;
        Servings = recipe.Servings;
        CookDuration = recipe.CookDuration;
    }
    
    private readonly IngredientGroup _ingredientGroup;
    private readonly CookingTechnic _cookingTechnic;
    private List<CookingStep> _cookingSteps;

    public void AddCookingStep(CookingStep cookingStep)
    {
        _cookingTechnic.AddStep(cookingStep);
    }

    public void RemoveCookingStep(CookingStep cookingStep)
    {
        _cookingTechnic.RemoveCookingStep(cookingStep);
    }

    public void SetCookingStep(int setIndex, CookingStep cookingStep)
    {
        _cookingTechnic.SetCookingStep(setIndex, cookingStep);
    }

    public void InsertCookingStep(int insertIndex, CookingStep cookingStep)
    {
        _cookingTechnic.InsertCookingStep(insertIndex, cookingStep);
    }

    public void UpdateIngredient(Ingredient ingredient)
    {
        ArgumentNullException.ThrowIfNull(ingredient);
        _ingredientGroup.Update(ingredient);
    }

    public void AddIngredient(Ingredient ingredient)
    {
        ArgumentNullException.ThrowIfNull(ingredient);

        _ingredientGroup.Add(ingredient);
    }

    public void RemoveIngredient(Ingredient ingredient)
    {
        ArgumentNullException.ThrowIfNull(ingredient);

        _ingredientGroup.Remove(ingredient);
    }

    public void UpdateTitle(string title)
    {
        Title = title;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    public void UpdateServings(int servings)
    {
        Servings = servings;
    }

    public void UpdateCookDuration(TimeSpan cookDuration)
    {
        CookDuration = cookDuration;
    }

    public void UpdateEnergyValue(EnergyValue energyValue)
    {
        _energyValue = energyValue;
    }

    public List<CookingStep> CookingSteps
    {
        get => _cookingSteps;
        set {}
    }

    public IngredientGroup Ingredients
    {
        get => _ingredientGroup;
        set {}
    }

    public static implicit operator RecipeDataBaseObject(Recipe recipe) => new(recipe);

    public static explicit operator Recipe(RecipeDataBaseObject recipeDBO)
    {
        var recipe = new Recipe(recipeDBO.Id, recipeDBO.Title, recipeDBO.Description, recipeDBO.Servings,
            recipeDBO.CookDuration, recipeDBO.EnergyValue, recipeDBO._ingredientGroup, recipeDBO._cookingTechnic);
        return recipe;
    }
}