using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository.Core.Repository
{
    public interface ICustomerRepository : IUnDeleteableRepository<Customer>
    {
        Customer AnonymousCustomer { get; }
    }
}
