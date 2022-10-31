using Recipes.Domain.ValueObjects;
using Recipes.Tests.Shared.Builders;

namespace Recipes.Tests.Shared.BuilderEntries;

public static class An
{
    public static EntityId EntityId => EntityId.NewId();
    public static IngredientBuilder Ingredient => new();
}