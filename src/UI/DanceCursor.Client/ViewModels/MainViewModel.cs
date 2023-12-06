using DanceCursor.Domain.Abstractions;
using DanceCursor.Shared.ViewModels;
using Prism.Regions;
using Reactive.Bindings;
using System.Reactive.Linq;

namespace DanceCursor.Client.ViewModels
{
    public class MainViewModel : NavigationViewModelBase
    {
        private readonly IMouse _mouse;
        private readonly IRegionManager _regionManager;

        public MainViewModel(
            IMouse mouse, 
            IRegionManager regionManager) 
        {
            _regionManager = regionManager;
            _mouse = mouse;

            EnableDanceCursor
                .Subscribe(_ =>
                {
                    _mouse.Move();
                });
        }

        /// <summary>
        /// Show window authorization.
        /// </summary>
        public ReactiveCommand EnableDanceCursor { get; } = new();
    }
}