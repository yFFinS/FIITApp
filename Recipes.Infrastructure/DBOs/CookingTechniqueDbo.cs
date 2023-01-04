using System.Xml.Serialization;
using Recipes.Domain.Entities.RecipeAggregate;

namespace Recipes.Infrastructure;

[XmlType(nameof(CookingTechnic))]
public class CookingTechniqueDbo
{
    public string[] Steps { get; set; }
};