using Avalonia.Controls;
using ReactiveUI;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities.ProductAggregate;
using Recipes.Domain.Entities.RecipeAggregate;
using Recipes.Domain.IngredientsAggregate;
using Recipes.Domain.Interfaces;
using Recipes.Domain.ValueObjects;
using Recipes.Presentation.DataTypes;
using Recipes.Presentation.Exceptions;
using Recipes.Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;

namespace Recipes.Presentation.ViewModels;

public class RecipeEditorViewModel : ViewModelBase
{
#if DEBUG
    public RecipeEditorViewModel()
    {
    }
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

    public string ImageUrl { get; set; }

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

    public IReadOnlyList<QuantityUnit> Units { get; private set; }

    public RecipeEditorViewModel(IRecipeRepository recipeRepository, IProductRepository productRepository, IViewContainer viewContainer, RecipeViewFactory factory,
        IExceptionContainer exceptionContainer)
    {
        RecipeRepository = recipeRepository;
        ProductRepository = productRepository;
        Title = "";
        Ingredients = new ObservableCollection<Ingredient>(new List<Ingredient>());
        Description = "";
        Servings = 0;
        CookingSteps = new ObservableCollection<CookingStep>(new List<CookingStep>());
        Units = new List<QuantityUnit>();

        PopulateProducts = (s) => ProductRepository.GetProductsByPrefix(s!.ToLower());
        SelectProduct = (search, item) =>
        {
            Units = item.ValidQuantityUnits;
            return item.Name;
        };

        AddCookingStepCommand = ReactiveCommandExtended.Create<TextBox>(AddCookingStep, exceptionContainer);
        RemoveCookingStepCommand = ReactiveCommandExtended.Create<CookingStep>(RemoveCookingStep, exceptionContainer);
        AddIngredientCommand = ReactiveCommandExtended.Create<Grid>(AddIngredient, exceptionContainer);
        RemoveIngredientCommand = ReactiveCommandExtended.Create<Ingredient>(RemoveIngredient, exceptionContainer);
        SaveRecipeCommand =
            ReactiveCommandExtended.Create(() => SaveRecipe(viewContainer, factory), exceptionContainer);

        GetProductNameCommand = ReactiveCommandExtended.Create<EntityId>(
            id => productRepository.GetProductById(id), exceptionContainer);
    }

    public ReactiveCommand<TextBox, Unit> AddCookingStepCommand { get; }

    public ReactiveCommand<CookingStep, Unit> RemoveCookingStepCommand { get; }

    public ReactiveCommand<Grid, Unit> AddIngredientCommand { get; }

    public ReactiveCommand<Ingredient, Unit> RemoveIngredientCommand { get; }

    public ReactiveCommand<Unit, Unit> SaveRecipeCommand { get; }

    public ReactiveCommand<EntityId, Unit> GetProductNameCommand { get; }

    private void AddCookingStep(TextBox description)
    {
        CookingSteps.Add(new CookingStep($"{CookingSteps.Count + 1}. {description.Text}"));
        description.Text = "";
    }

    private void RemoveCookingStep(CookingStep step)
    {
        CookingSteps.Remove(step);
    }

    public Func<string?, IEnumerable<object>> PopulateProducts { get; }

    public AutoCompleteSelector<Product> SelectProduct { get; }

    private void AddIngredient(Grid parent)
    {
        if (parent.Children[0] is not AutoCompleteBox box)
            return;
        if (parent.Children[1].LogicalChildren[0] is not NumericUpDown num)
            return;
        if (parent.Children[1].LogicalChildren[1] is not ComboBox combo)
            return;

        if (CurrentProduct is null)
        {
            var ex = new RecipeEditorException("Выберете продукт");
            // DataValidationErrors.SetError(box, ex);
            throw ex;
        }

        if (CurrentUnit is null)
        {
            var ex = new RecipeEditorException("Выберете единицу измерения");
            // DataValidationErrors.SetError(combo, ex);
            throw ex;
        }

        Ingredients.Add(new Ingredient(CurrentProduct, new Quantity(CurrentCount, CurrentUnit)));

        box.Text = "";
        CurrentProduct = null;
        box.Focus();
        CurrentCount = 1;
        CurrentUnit = null;
    }

    private void RemoveIngredient(Ingredient ingredient)
    {
        Ingredients.Remove(ingredient);
    }

    private void SaveRecipe(IViewContainer container, RecipeViewFactory factory)
    {
        if (Ingredients.Count == 0)
            throw new RecipeEditorException("Запишите ингредиенты");
        if (Servings == 0)
            throw new RecipeEditorException("Назначьте количество порций");
        if (CookDuration == TimeSpan.Zero)
            throw new RecipeEditorException("Выберите время приготовления");
        if (CookingSteps.Count == 0)
            throw new RecipeEditorException("Напишите шаги приготовления");

        var recipe = new Recipe(EntityId.NewId(), Title, Description, Servings, CookDuration);
        recipe.ImageUrl = new Uri(ImageUrl);
        foreach (var ingr in Ingredients)
            recipe.AddIngredient(ingr);
        foreach (var cookingStep in CookingSteps)
            recipe.AddCookingStep(cookingStep);
        RecipeRepository.AddRecipes(new[] { recipe }, useUserDatabase: true);
        container.Content = factory.Create(recipe, this);
    }
}