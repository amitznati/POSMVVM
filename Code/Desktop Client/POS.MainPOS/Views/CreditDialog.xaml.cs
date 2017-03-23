using MahApps.Metro.Controls;
using POS.MainPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace POS.MainPOS.Views
{
    /// <summary>
    /// Interaction logic for CreditDialog.xaml
    /// </summary>
    public partial class CreditDialog : MetroWindow
    {
        CreditDialogViewModel viewModel = new CreditDialogViewModel();
        public CreditDialog()
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
    }
}
