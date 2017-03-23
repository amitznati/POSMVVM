using Caliburn.Metro.Demo.ViewModels;
using POS.Data;
using POSEntities.Entities;
using System.Collections.ObjectModel;

namespace POS_WPF.ViewModels
{
    public class MainPOSViewModel : FlyoutBaseViewModel
    {
        public MainPOSViewModel()
        {
            Basket = new ObservableCollection<Order>();
            this.Position = MahApps.Metro.Controls.Position.Right;
        }

        public ObservableCollection<Order> Basket { get; private set; }
    }
}
