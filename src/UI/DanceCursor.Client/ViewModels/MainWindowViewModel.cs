using DanceCursor.Domain.Abstractions;
using DanceCursor.Shared.ViewModels;
using Prism.Regions;
using Reactive.Bindings;
using System.Reactive.Linq;

namespace DanceCursor.Client.ViewModels
{
    public class MainWindowViewModel : NavigationViewModelBase
    {
        private readonly IMouse _mouse;
        private readonly IRegionManager _regionManager;

        public bool SmartMode { get; set; }
        public bool SimpleMode { get; set; }

        public MainWindowViewModel(
            IMouse mouse,
            IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _mouse = mouse;

            EnableDanceCursor
                .Subscribe(_ =>
                {
                    if (SmartMode)
                        _mouse.SmartMove();
                    if (SimpleMode)
                        _mouse.Move();
                });

            DisableDanceCursor
                .Subscribe(_ =>
                {

                });
        }

        /// <summary>
        /// Show window authorization.
        /// </summary>
        public ReactiveCommand EnableDanceCursor { get; } = new();

        public ReactiveCommand DisableDanceCursor { get; } = new();
    }
}