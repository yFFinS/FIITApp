using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
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
    public RecipeEditorViewModel() { }
#endif
    public IRecipeRepository RecipeRepository { get; }
    public IProductRepository ProductRepository { get; }

    #region Ingredient

    public Product CurrentProduct
    {
        get => _currentProduct;
        set => this.RaiseAndSetIfChanged(ref _currentProduct, value);
    }

    public float CurrentCount
    {
        get => _currentCount;
        set => this.RaiseAndSetIfChanged(ref _currentCount, value);
    }

    public QuantityUnit CurrentUnit
    {
        get => _currentUnit;
        set => this.RaiseAndSetIfChanged(ref _currentUnit, value);
    }

    #endregion

    public string Title { get; set; }

    public ObservableCollection<Ingredient> Ingredients { get; set; }

    public string Description { get; set; }

    #region CookingTime

    private int _hours;
    private int _minutes;
    private int _seconds;
    private Product _currentProduct;
    private float _currentCount = 1;
    private QuantityUnit _currentUnit;

    public int Hours
    {
        get => CookDuration.Hours;
        set
        {
            var delta = new TimeSpan(0, value - _hours, 0, 0);
            CookDuration += delta;
            this.RaiseAndSetIfChanged(ref _hours, CookDuration.Hours);
        }
    }
    public int Minutes
    {
        get => CookDuration.Minutes;
        set
        {
            var delta = new TimeSpan(0, 0, value - _minutes, 0);
            CookDuration += delta;
            this.RaiseAndSetIfChanged(ref _minutes, CookDuration.Minutes);
        }
    }
    public int Seconds
    {
        get => CookDuration.Seconds;
        set
        {
            var delta = new TimeSpan(0, 0, 0, value - _seconds);
            CookDuration += delta;
            this.RaiseAndSetIfChanged(ref _seconds, CookDuration.Seconds);
        }
    }

    #endregion

    public TimeSpan CookDuration { get; set; }

    public int Servings { get; set; }

    public ObservableCollection<CookingStep> CookingSteps { get; set; }

    public IReadOnlyList<QuantityUnit> Units { get; }

    public RecipeEditorViewModel(IRecipeRepository recipeRepository, IProductRepository productRepository, IQuantityUnitRepository unitRepository)
    {
        RecipeRepository = recipeRepository;
        ProductRepository = productRepository;
        Title = "";
        Ingredients = new ObservableCollection<Ingredient>(new List<Ingredient>());
        Description = "";
        Servings = 0;
        CookingSteps = new ObservableCollection<CookingStep>(new List<CookingStep>());
        Units = unitRepository.GetAllUnits();

        PopulateProducts = async (s, token) => await ProductRepository.GetProductsByPrefixAsync(s!.ToLower());
        SelectProduct = (search, item) => item.Name;

        AddCookingStepCommand = ReactiveCommand.Create<TextBox>(AddCookingStep);
        AddIngredientCommand = ReactiveCommand.Create<Grid>(AddIngredient);
        RemoveIngredientCommand = ReactiveCommand.Create<Ingredient>(RemoveIngredient);
        SaveRecipeCommand = ReactiveCommand.Create(SaveRecipe);

        GetProductNameCommand = ReactiveCommand.CreateFromTask<EntityId, Product>(
                id => productRepository.GetProductByIdAsync(id));
    }

    public ReactiveCommand<TextBox, Unit> AddCookingStepCommand { get; }

    public ReactiveCommand<Grid, Unit> AddIngredientCommand { get; }

    public ReactiveCommand<Ingredient, Unit> RemoveIngredientCommand { get; }

    public ReactiveCommand<Unit, Unit> SaveRecipeCommand { get; }

    public ReactiveCommand<EntityId, Product> GetProductNameCommand { get; }

    private void AddCookingStep(TextBox description)
    {
        CookingSteps.Add(new CookingStep($"{CookingSteps.Count + 1}. {description.Text}"));
        description.Text = "";
    }

    public Func<string?, CancellationToken, Task<IEnumerable<object>>> PopulateProducts { get; }

    public AutoCompleteSelector<Product> SelectProduct { get; }

    private void AddIngredient(Grid parent)
    {
        if (CurrentProduct is null)
            //TODO
            throw new Exception();
        Ingredients.Add(new Ingredient(CurrentProduct, new Quantity(CurrentCount, CurrentUnit)));
        if (parent.Children[0] is AutoCompleteBox box)
        {
            box.Text = "";
            CurrentProduct = null;
            box.Focus();
        }

        if (parent.Children[1] is NumericUpDown num)
            CurrentCount = 1;
        if (parent.Children[2] is ComboBox combo)
            CurrentUnit = null;
    }

    private void RemoveIngredient(Ingredient ingredient)
    {
        Ingredients.Remove(ingredient);
    }

    private void SaveRecipe()
    {
        var recipe = new Recipe(EntityId.NewId(), Title, Description, Servings, CookDuration);
        foreach (var ingr in Ingredients)
            recipe.AddIngredient(ingr);
        foreach (var cookingStep in CookingSteps)
            recipe.AddCookingStep(cookingStep);
        RecipeRepository.AddRecipesAsync(new[] { recipe });
    }
}