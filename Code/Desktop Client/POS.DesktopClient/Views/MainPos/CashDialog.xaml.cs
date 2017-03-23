using MahApps.Metro.Controls;
using POS.DesktopClient.ViewModels.MainPos;
using System.Windows;
using System.Windows.Controls;

namespace POS.DesktopClient.Views.MainPos
{
    /// <summary>
    /// Interaction logic for CashDialog.xaml
    /// </summary>
    public partial class CashDialog : MetroWindow
    {
        CashDialogViewModel viewModel = new CashDialogViewModel();
        public double ToPay { get { return viewModel.ToPay; } set { viewModel.ToPay = value; } }
        public double Payed { get { return viewModel.Payed; } set { viewModel.Payed = value; } }
        public double Change { get { return viewModel.Change; } set { viewModel.Change = value; } }
        public double LeftToPay { get { return viewModel.LeftToPay; } set { viewModel.LeftToPay = value; } }
        public CashDialog()
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double s = double.Parse(((Button)sender).CommandParameter.ToString());
            viewModel.MoneyClick(s);
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
