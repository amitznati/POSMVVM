using MahApps.Metro.Controls;
using POS.Data.Repository;
using POSEntities.Entities;
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

namespace POS.DesktopClient.Views.General
{
    /// <summary>
    /// Interaction logic for SelectEmployeeInShiptDialog.xaml
    /// </summary>
    public partial class SelectEmployeeInShiptDialog : MetroWindow
    {
        public SelectEmployeeInShiptDialog()
        {
            this.DataContext = UnitOfWork.get().DailyAttendances.GetAllInShifp();
            InitializeComponent();
        }
        public Employee SelectedEmployee { get; private set; }
        private void btnEmployee_Click(object sender, RoutedEventArgs e)
        {
            SelectedEmployee = ((DailyAttendance)((Button)sender).DataContext).Employee;
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
