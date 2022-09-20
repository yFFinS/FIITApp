using Recipes.Tests.Shared.Builders;

namespace Recipes.Tests.Shared.BuilderEntries;

public static class A
{
    public static RecipeBuilder Recipe => new();
    public static ProductBuilder Product => new();
}