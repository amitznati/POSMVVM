using POS.Data.Repository;
using POS.MainPOS.Events;
using POS.MainPOS.Views;
using POS.Windows;
using POSEntities;
using POSEntities.Entities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POS.MainPOS.ViewModels
{
    public class DialogContectViewModel:BindableViewModelBase
    {
        public DialogContectViewModel()
        {
            ContentView = new SelectDisplayableItemView();
            SelectCommand = new DelegateCommand(SelectExecute, CanSelectExecute);
            CancelCommand = new DelegateCommand(CancelExecute);
            Menus = UnitOfWork.get().Menus.GetAll().ToList<Menu>();
            Products = UnitOfWork.get().Products.GetAll().ToList<Product>();

        }

        private bool CanSelectExecute()
        {
            return SelectedItem != null;
        }

        public List<Menu> Menus
        {
            get { return GetValue(() => Menus); }
            set { SetValue(() => Menus, value); }
        }

        public List<Product> Products
        {
            get { return GetValue(() => Products); }
            set { SetValue(() => Products, value); }
        }

        public IDisplayableModel SelectedItem
        { get { return GetValue(() => SelectedItem); } set { SetValue(() => SelectedItem, value); SelectCommand.RaiseCanExecuteChanged(); } }
        private void SelectExecute()
        {
            Globals.get().EventAggregator.GetEvent<SelectItemComplete>().Publish(SelectedItem);
        }

        private void CancelExecute()
        {
            Globals.get().EventAggregator.GetEvent<SelectItemCanceled>().Publish(null);
        }
        public DelegateCommand SelectCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public FrameworkElement ContentView
        {
            get { return GetValue(() => ContentView); }
            set { SetValue(() => ContentView, value); }
        }
    }
}
