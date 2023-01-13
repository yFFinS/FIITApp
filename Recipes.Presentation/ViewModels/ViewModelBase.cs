using ReactiveUI;

namespace Recipes.Presentation.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public virtual void Refresh() { }
    }
}