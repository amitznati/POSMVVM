using System.ComponentModel.Composition;
using Caliburn.Micro;
using POS_WPF.ViewModels;

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
            this.flyouts.Add(new MainPOSViewModel());
            this.flyouts.Add(new DataListViewModel());
        }

        public string btnText { get { return "fsdgfgdf"; } }
    }
}