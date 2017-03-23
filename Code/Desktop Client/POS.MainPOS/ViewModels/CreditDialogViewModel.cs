using POS.Windows;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MainPOS.ViewModels
{
    public class CreditDialogViewModel:BindableViewModelBase
    {
        public DelegateCommand ManualCommand { get; set; }
        public DelegateCommand AcceptCommand { get; set; }
        public CalculatorViewModel CalcVM { get { return GetValue(() => CalcVM); } set { SetValue(() => CalcVM, value); } }
        public bool IsManual { get { return GetValue(() => IsManual); } set { SetValue(() => IsManual, value); } }
        public CreditDialogViewModel()
        {
            CalcVM = new CalculatorViewModel();
            ManualCommand = new DelegateCommand(ManualExecute);
            AcceptCommand = new DelegateCommand(AcceptAcecute);
        }
        private void ManualExecute()
        {
            IsManual = !IsManual;
        }
        private void AcceptAcecute()
        {

        }

    }
}
