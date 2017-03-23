using POS.Windows;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.DesktopClient.ViewModels.MainPos
{
    public class CashDialogViewModel:BindableViewModelBase
    {
        public double ToPay { get { return GetValue(() => ToPay); } set { SetValue(() => ToPay, value);  } }
        public double Payed { get { return GetValue(() => Payed); } set { SetValue(() => Payed, value); } }
        public double Change { get { return GetValue(() => Change); } set { SetValue(() => Change, value); } }
        public double LeftToPay { get { return GetValue(() => LeftToPay); } set { SetValue(() => LeftToPay, value); } }
        public CalculatorViewModel CalcVM { get { return GetValue(() => CalcVM); } set { SetValue(() => CalcVM, value); } }

        DelegateCommand<string> MoneyClickCommand { get; set; }
        public DelegateCommand SumFromCalc { set; get; }
        public DelegateCommand ClearCommand { set; get; }

        public CashDialogViewModel()
        {
            CalcVM = new CalculatorViewModel();
            MoneyClickCommand = new DelegateCommand<string>(MoneyClick);
            ClearCommand = new DelegateCommand(Clear);
            SumFromCalc = new DelegateCommand(FromCalc);
        }
        private void Clear()
        {
            setPayed(0);
        }
        public void MoneyClick(string _money)
        {
            double money = double.Parse(_money);
            MoneyClick(money);
        }
        public void MoneyClick(double money)
        {
            setPayed(Payed + money);

        }
        private void FromCalc()
        {
            double fromCalc = 0;
            double.TryParse(CalcVM.TBText, out fromCalc);
            MoneyClick(fromCalc);
        }
        private void setPayed(double payed)
        {
            Payed = payed;
            LeftToPay = Math.Max(0, ToPay - Payed);
            Change = Math.Max(0, Payed - ToPay);
        }
        
    }
}
