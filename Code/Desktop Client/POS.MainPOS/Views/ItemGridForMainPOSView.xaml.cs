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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POS.MainPOS.Views
{
    /// <summary>
    /// Interaction logic for ItemGridForMainPOSView.xaml
    /// </summary>
    public partial class ItemGridForMainPOSView : UserControl
    {
        public ItemGridForMainPOSView()
        {
            InitializeComponent();
        }

        private void CurrentItem_Click(object sender, RoutedEventArgs e)
        {
            ItemDisplayInfo item = ((Button)sender).DataContext as ItemDisplayInfo;
            Globals.get().EventAggregator.GetEvent<Events.POSItemClick>().Publish(item);
        }
    }
}
