using Avalonia.Web.Blazor;

namespace Recipes.Presentation.Web;

public partial class App
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        WebAppBuilder.Configure<Recipes.Presentation.App>()
            .SetupWithSingleViewLifetime();
    }
}