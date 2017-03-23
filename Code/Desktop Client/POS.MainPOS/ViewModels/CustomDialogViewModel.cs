using POS.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POS.MainPOS.ViewModels
{
    public class CustomDialogViewModel:BindableViewModelBase
    {
        public CustomDialogViewModel()
        {
            DialogContectViewModel = new DialogContectViewModel();
        }
        public DialogContectViewModel DialogContectViewModel
        {
            get { return GetValue(() => DialogContectViewModel); }
            set { SetValue(() => DialogContectViewModel, value); }
        }

    }
}
