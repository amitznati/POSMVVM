using System.ComponentModel.Composition;
using Caliburn.Metro.Demo.ViewModels;
using Caliburn.Micro;
using Caliburn.Metro.Demo.ViewModels.Flyouts;

namespace Caliburn.Metro.Demo.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Screen, IShell
    {
        private readonly IObservableCollection<FlyoutBaseViewModel> flyouts =
            new BindableCollection<FlyoutBaseViewModel>();

        public IObservableCollection<FlyoutBaseViewModel> Flyouts
        {
            get
            {
                return this.flyouts;
            }
        }

        public void Close()
        {
            this.TryClose();
        }

        public void ToggleFlyout(int index)
        {
            var flyout = this.flyouts[index];
            flyout.IsOpen = !flyout.IsOpen;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            this.DisplayName = "Caliburn.Metro.Demo";
            //this.flyouts.Add(new DataListViewModel());
            this.flyouts.Add(new POSViewModel());
        }
    }
}