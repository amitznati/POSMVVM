using MahApps.Metro.Controls;
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

namespace POS.DesktopClient.Views.MainPos
{
    /// <summary>
    /// Interaction logic for SuspendedOrdersDialog.xaml
    /// </summary>
    public partial class SuspendedOrdersDialog : MetroWindow
    {
        public List<Order> SuspendedOrders { get; set; }
        public Order SelectedOrder { get; set; }
        public SuspendedOrdersDialog(List<Order> orders)
        {
            SuspendedOrders = orders;
            this.DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
