using POS.Data.Repository.Core.Repository;
using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository.Repositories
{
    public class CustomerRepository : UnDeleteableRepository<Customer>, ICustomerRepository
    {
        POSContext _context;
        public CustomerRepository(POSContext context) : base(context) { _context = context; }

        public Customer AnonymousCustomer
        {
            get {return _context.Set<Customer>().Where(c => c.FirstName == "לקוח" && c.LastName == "כללי").FirstOrDefault(); }
        }
    }
}
