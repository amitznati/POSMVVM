
using POS.Data.Repository.Core.Repository;
using POSEntities.Entities;
using System;

namespace POS.Data.Repository
{
   
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }   
        IEmployeeRepository Employees { get; }
        IPeopleRepository Peoples { get; }
        IRoleRepository Roles { get; }
        IDepartmentRepository Departments { get; }
        IGroupRepository Groups { get; }
        ICustomerRepository Customers { get; }
        IVendorRepository Vendors { get; }
        IOrderRepository Orders { get; }
        Repository<Tax> Taxes { get; }
        IDailyAttendanceRepository DailyAttendances { get; }
        int Complete();
    }
}