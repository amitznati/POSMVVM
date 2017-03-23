using MahApps.Metro.Controls;
using POS.DesktopClient.ViewModels.MainPos;
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

namespace POS.DesktopClient.Views.MainPos
{
    /// <summary>
    /// Interaction logic for SelectDiaplyableItemDialog.xaml
    /// </summary>
    public partial class SelectDiaplyableItemDialog : MetroWindow
    {
        SelectDiaplyableItemDialogViewModel viewModel = new SelectDiaplyableItemDialogViewModel();
        public SelectDiaplyableItemDialog()
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
        public object SelectedItem { get; set; }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            SelectedItem = viewModel.SelectedItem;
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
