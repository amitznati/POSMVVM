using MahApps.Metro.Controls;
using POS.DesktopClient.ViewModels.MainPos;
using POSEntities.Entities;

namespace POS.DesktopClient.Views.MainPos
{
    /// <summary>
    /// Interaction logic for SelectPersonDialog.xaml
    /// </summary>
    public partial class SelectPersonDialog : MetroWindow
    {
        SelectPersonDialogViewModel viewModel = new SelectPersonDialogViewModel();
        public Person SelectedPerson { get { return viewModel.SelectedObject; } }
        public SelectPersonDialog()
        {

            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.SelectExecute();
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
