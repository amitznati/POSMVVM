using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace POS.DesktopClient.ViewModels.Management
{
    public interface IEntityListViewModelBase
    {
        void SaveNewEntity(object entity);
        void RemoveEntity(object entity);
        FrameworkElement getEntityView();
    }
}
