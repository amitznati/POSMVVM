using POS.Data.Repository;
using POS.Windows;
using POSEntities.Entities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.DesktopClient.ViewModels.MainPos
{
    public class SelectDiaplyableItemDialogViewModel:BindableViewModelBase
    {
        public List<Product> Products { set; get; }
        public List<Menu> Menus
        {
            get { return GetValue(() => Menus); }
            set { SetValue(() => Menus, value); }
        }
        public object SelectedItem
        {
            get { return GetValue(() => SelectedItem); }
            set { SetValue(() => SelectedItem, value); }
        }
        public SelectDiaplyableItemDialogViewModel()
        {
            Products = UnitOfWork.get().Products.GetAll().ToList();
            Menus = UnitOfWork.get().Menus.GetAll().ToList();

        }
    }
}
