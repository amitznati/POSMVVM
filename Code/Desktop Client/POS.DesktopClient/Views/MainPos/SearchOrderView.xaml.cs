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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POS.DesktopClient.Views.MainPos
{
    /// <summary>
    /// Interaction logic for SearchOrderView.xaml
    /// </summary>
    public partial class SearchOrderView : UserControl
    {
        public SearchOrderView()
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }
        SearchOrderDialogViewModel viewModel = new SearchOrderDialogViewModel();
        private void CheckComboBox_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            viewModel.RefreshDepartment();
        }

        private void CheckComboBox_ItemSelectionChanged_1(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            viewModel.RefreshGroups();
        }

        private void CheckComboBox_ItemSelectionChanged_2(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {

        }
    }
}
