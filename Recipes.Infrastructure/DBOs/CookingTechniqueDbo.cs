using Recipes.Domain.Entities.RecipeAggregate;
using System.Xml.Serialization;

namespace Recipes.Infrastructure;

[XmlType(nameof(CookingTechnic))]
public class CookingTechniqueDbo
{
    public string[] Steps { get; set; }
};