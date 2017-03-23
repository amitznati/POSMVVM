
using MahApps.Metro.Controls;
using POS_WPF;
using Prism.Mvvm;

namespace Caliburn.Metro.Demo.ViewModels
{
    public abstract class FlyoutBaseViewModel : BindableViewModelBase
    {
        public string Header
        {
            get { return GetValue(() => Header); }
            set { SetValue(() => Header, value); }
        }

        public bool IsOpen
        {
            get { return GetValue(() => IsOpen); }
            set { SetValue(() => IsOpen, value); }
        }

        public Position Position
        {
            get { return GetValue(() => Position); }
            set { SetValue(() => Position, value); }
        }
    }
}