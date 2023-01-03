namespace Recipes.Infrastructure;

public record ProductDbo(string Id, string Name, string? Description,
    double? PieceWeight, string? ImageUrl, int[] QuantityUnitIds)
{
    public ProductDbo(string id, string name) : this(id, name, null, null, null, Array.Empty<int>())
    {
    }
}