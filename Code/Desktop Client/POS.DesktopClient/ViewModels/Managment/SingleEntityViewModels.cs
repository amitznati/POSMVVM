using POS.Data.Repository;
using POSEntities.Entities;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace POS.DesktopClient.ViewModels.Management
{
    public class ContactViewModel : EntityViewModelBase<Contact>
    {
        public ContactViewModel()
            : base()
        {

        }
        public override void InitiatEntity()
        {
            Entity = new Contact() { Mode = true, Address = new Address() };
            base.InitiatEntity();
        }
    }
    public class DepartmentViewModel : EntityViewModelBase<Department>
    {
        public DepartmentViewModel()
            : base()
        {

        }
        public override void InitiatEntity()
        {
            Entity = new Department() { Mode = true};
            base.InitiatEntity();
        }
    }
    public class VendorViewModel : EntityViewModelBase<Vendor>
    {
        public VendorViewModel()
            : base()
        {
        }
        public override void InitiatEntity()
        {
            Entity = new Vendor() { Mode =true, Address = new Address() };
            if (IsToShowEditButtons)
                Entity.Contacts = new Repository<Contact>(UnitOfWork.get().Context).GetAll().ToList<Contact>();
            base.InitiatEntity();
        }

    }
    public class ProductViewModel : EntityViewModelBase<Product>
    {

        public List<Department> Departments
        {
            get { return GetValue(() => Departments); }
            set { SetValue(() => Departments, value); }
        }

        public List<Group> Groups
        {
            get { return GetValue(() => Groups); }
            set { SetValue(() => Groups, value); }
        }

        public List<Vendor> Vendors
        {
            get { return GetValue(() => Vendors); }
            set { SetValue(() => Vendors, value); }
        }


        public ProductViewModel()
            : base()
        {
            Departments = (List<Department>)UnitOfWork.get().Departments.GetAll();
            Groups = (List<Group>)UnitOfWork.get().Groups.GetAll();
            Vendors = (List<Vendor>)UnitOfWork.get().Vendors.GetAll();
        }

        public override void InitiatEntity()
        {
            Entity = new Product() { mode = true };
            base.InitiatEntity();
        }
    }
    public class EmployeeViewModel : EntityViewModelBase<Employee>
    {
        public EmployeeViewModel()
            : base()
        {
            Rolement = (List<Role>)UnitOfWork.get().Roles.GetAll();
            SaleryTypes = (List<SaleryType>)UnitOfWork.get().SaleryTypes.GetAll();
        }

        public List<Role> Rolement
        {
            get { return GetValue(() => Rolement); }
            set { SetValue(() => Rolement, value); }
        }

        public List<SaleryType> SaleryTypes
        {
            get { return GetValue(() => SaleryTypes); }
            set { SetValue(() => SaleryTypes, value); }
        }

        public override void InitiatEntity()
        {
            Entity = new Employee() { Mode = true, Address = new Address() };
            base.InitiatEntity();
        }


    }
    public class CustomerViewModel : EntityViewModelBase<Customer>
    {
        public CustomerViewModel()
            : base()
        {
        }
        public override void InitiatEntity()
        {
            Entity = new Customer() { Mode = true, Address = new Address() };
            base.InitiatEntity();
        }
    }
    public class GroupViewModel:EntityViewModelBase<Group>
    {
        public GroupViewModel():base()
        {
            Departments = (List<Department>)UnitOfWork.get().Departments.GetAll();
        }
        public List<Department> Departments
        {
            get { return GetValue(() => Departments); }
            set { SetValue(() => Departments, value); }
        }

        public override void InitiatEntity()
        {
            Entity = new Group() { Mode = true };
            base.InitiatEntity();
        }
    }

    public class MenuViewModel:EntityViewModelBase<Menu>
    {
        public MenuViewModel():base()
        {
            Entity = new Menu();
            base.InitiatEntity();
        }
        public override void InitiatEntity()
        {
            base.InitiatEntity();
        }
    }
}
