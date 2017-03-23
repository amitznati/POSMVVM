using POS_WPF.Views;
using POSEntities.Entities;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace POS_WPF.ViewModels
{
    public class ContactListViewModel : EntityListViewModelBase<Contact>
    {
        public ContactListViewModel() : base() { EntityViewModel = new ContactViewModel(); }
        public override System.Windows.FrameworkElement getEntityView()
        {
            return new ContactView() { DataContext = new ContactViewModel() };
        }
    }
    public class CustomerListViewModel : EntityListViewModelBase<Customer>
    {
        public CustomerListViewModel(){EntityViewModel = new CustomerViewModel();}
        public override System.Windows.FrameworkElement getEntityView()
        {
            return new CustomerView() { DataContext = new CustomerViewModel() };
        }
    }
    public class DepartmentListViewModel : EntityListViewModelBase<Department>
    {
        public DepartmentListViewModel() : base() { EntityViewModel = new DepartmentViewModel(); }
        public override System.Windows.FrameworkElement getEntityView()
        {
            return new DepartmentView() { DataContext = new DepartmentViewModel() };
        }
    }
    public class EmployeeListViewModel : EntityListViewModelBase<Employee>
    {
        public EmployeeListViewModel()
            : base()
        {
            EntityViewModel = new EmployeeViewModel();
        }
        public override System.Windows.FrameworkElement getEntityView()
        {
            return new EmployeeView() { DataContext = new EmployeeViewModel() };
        }
    }
    public class GroupListViewModel:EntityListViewModelBase<Group>
    {
        public GroupListViewModel() : base() 
        {
            EntityViewModel = new GroupViewModel();
        }
        protected override void initEntityList()
        {
            base.initEntityList();
            
            EntityList.GroupDescriptions.Add(new PropertyGroupDescription("Department.DeptName"));
        }
        public override System.Windows.FrameworkElement getEntityView()
        {
            return new GroupView() { DataContext = new GroupViewModel() };
        }
    }
    public class ProductListViewModel : EntityListViewModelBase<Product>
    {

        public ProductListViewModel()
            : base()
        {
            EntityViewModel = new ProductViewModel();
        }
        protected override void initEntityList()
        {
            base.initEntityList();
            EntityList.GroupDescriptions.Add(new PropertyGroupDescription("Department.DeptName"));
            EntityList.GroupDescriptions.Add(new PropertyGroupDescription("Group.GroupName"));
        }
        public override System.Windows.FrameworkElement getEntityView()
        {
            return new ProductView() { DataContext = new ProductViewModel() };
        }
    }

    public class VendorListViewModel : EntityListViewModelBase<Vendor>
    {
        public VendorListViewModel()
            : base()
        {
            EntityViewModel = new VendorViewModel();
        }
        public override System.Windows.FrameworkElement getEntityView()
        {
            return new VendorView() { DataContext = new VendorViewModel() };
        }
    }

    public class MenuListViewModel:EntityListViewModelBase<Menu>
    {
        public MenuListViewModel():base()
        {
            EntityViewModel = new MenuViewModel();
        }
        public override System.Windows.FrameworkElement getEntityView()
        {
            return new MenuView() { DataContext = new MenuViewModel() };
        }
    }
}
