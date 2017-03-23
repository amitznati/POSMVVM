
using POS.Data.Repository.Core.Repository;
using POS.Data.Repository.Repositories;
using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository
{
    public  class UnitOfWork  :  IUnitOfWork
    {
        private readonly POSContext _context;
        private UnitOfWork(POSContext context)
        {
            Employees = new EmployeeRepository(context);
            Products = new ProductRepository(context);
            Peoples = new PeopleRepository(context);
            Roles = new RoleRepository(context);
            Customers = new CustomerRepository(context);
            Vendors = new VendorRepository(context);
            Departments = new DepartmentRepository(context);
            Groups = new GroupRepository(context);
            SaleryTypes = new Repository<SaleryType>(context);
            Menus = new Repository<Menu>(context);
            ItemDisplayInfoes = new Repository<ItemDisplayInfo>(context);
            Taxes = new Repository<Tax>(context);
            Orders = new OrderRepository(context);
            DailyAttendances = new DailyAttendanceRepository(context);
            _context = context;
        }
        private static volatile UnitOfWork instance = null;
        private static object syncRoot = new Object();
        public static UnitOfWork get()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new UnitOfWork(new POSContext());
                    }
                }
            }
            return instance;
        }
        
        public int Complete()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch(System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                throw ex;
            }
            catch(Exception exc)
            {
                throw exc;
            }
           
            
        }
        public POSContext Context { get{return _context;} }
        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
        public IEmployeeRepository Employees { get; private set; }
        public IProductRepository Products { get; private set; }
        public IPeopleRepository Peoples { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public IDepartmentRepository Departments { get; private set; }
        public IGroupRepository Groups { get; private set; }
        public ICustomerRepository Customers { get; private set; }
        public IVendorRepository Vendors { get; private set; }
        public Repository<SaleryType> SaleryTypes { get; private set; }
        public Repository<Menu> Menus { get; private set; }
        public Repository<ItemDisplayInfo> ItemDisplayInfoes { get; private set; }
        public IOrderRepository Orders { get; private set; }
        public Repository<Tax> Taxes { get; private set; }
        public IDailyAttendanceRepository DailyAttendances { get; private set; }
    }
}
