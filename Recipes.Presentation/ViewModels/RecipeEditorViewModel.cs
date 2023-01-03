using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;

namespace Recipes.Presentation.ViewModels;

internal class RecipeEditorViewModel : ViewModelBase
{
#if DEBUG
    public RecipeEditorViewModel()
    {
    }
#endif
    public IRecipeRepository RecipeRepository { get; }
    public IProductRepository ProductRepository { get; }

    #region Ingredient

    public Product CurrentProduct { get; set; }
    public float CurrentCount { get; set; }
    public QuantityUnit CurrentUnit { get; set; }

    #endregion

    public string Title { get; set; }

    public ObservableCollection<Ingredient> Ingredients { get; set; }

    public string Description { get; set; }

    public int Servings { get; set; }

    public ObservableCollection<CookingStep> CookingSteps { get; set; }

    public RecipeEditorViewModel(IRecipeRepository recipeRepository, IProductRepository productRepository)
    {
        RecipeRepository = recipeRepository;
        ProductRepository = productRepository;
        Title = "";
        Ingredients = new ObservableCollection<Ingredient>(new List<Ingredient>());
        Description = "";
        Servings = 0;
        CookingSteps = new ObservableCollection<CookingStep>(new List<CookingStep>());

        PopulateProducts = async (s, token) => await ProductRepository.GetProductsByPrefixAsync(s!.ToLower());
        SelectProduct = (search, item) => item.Name;

        AddCookingStepCommand = ReactiveCommand.Create<TextBox>(AddCookingStep);
        AddIngregientCommand = ReactiveCommand.Create<Grid>(AddIngredient);
        SaveRecipeCommand = ReactiveCommand.Create(SaveRecipe);
    }

    public ReactiveCommand<TextBox, Unit> AddCookingStepCommand { get; }
    public ReactiveCommand<Grid, Unit> AddIngregientCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveRecipeCommand { get; }

    private void AddCookingStep(TextBox description)
    {
        CookingSteps.Add(new CookingStep($"{CookingSteps.Count + 1}. {description.Text}"));
        description.Text = "";
    }

    public Func<string?, CancellationToken, Task<IEnumerable<object>>> PopulateProducts { get; }
    public AutoCompleteSelector<Product> SelectProduct { get; }

    private void AddIngredient(Grid parent)
    {
        Ingredients.Add(new Ingredient(CurrentProduct ?? new (EntityId.NewId(), "Unknown product"), new Quantity(CurrentCount, CurrentUnit)));
        if (parent.Children[0] is AutoCompleteBox box)
            box.Text = "";
        if (parent.Children[1] is NumericUpDown num)
            num.Value = 0;
        if (parent.Children[2] is ComboBox combo)
            combo.SelectedItem = new QuantityUnit("штуки", "шт");
    }

    private void SaveRecipe()
    {
        var recipe = new Recipe(EntityId.NewId(), Title, Description, Servings,TimeSpan.Zero);
        foreach (var ingr in Ingredients)
            recipe.AddIngredient(ingr);
        foreach (var cookingStep in CookingSteps)
            recipe.AddCookingStep(cookingStep);
        RecipeRepository.AddRecipesAsync(new[] { recipe });
    }
}