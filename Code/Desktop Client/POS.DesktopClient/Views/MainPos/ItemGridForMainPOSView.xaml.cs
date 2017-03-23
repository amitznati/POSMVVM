using POS.DesktopClient.Events.MainPosEvents;
using POSEntities.Entities;
using System.Windows;
using System.Windows.Controls;

namespace POS.DesktopClient.Views.MainPos
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
            Globals.get().EventAggregator.GetEvent<POSItemClick>().Publish(item);
        }
    }
}
