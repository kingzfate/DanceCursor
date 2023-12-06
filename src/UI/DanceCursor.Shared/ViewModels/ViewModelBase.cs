using Prism.Mvvm;
using Prism.Navigation;

namespace DanceCursor.Shared.ViewModels
{
    /// <inheritdoc cref="BindableBase" />
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        /// <inheritdoc />
        public virtual void Destroy()
        {
        }
    }
}