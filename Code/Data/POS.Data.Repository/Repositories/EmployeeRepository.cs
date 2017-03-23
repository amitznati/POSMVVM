
using POS.Data.Repository.Repositories;
using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository
{
    public class EmployeeRepository : UnDeleteableRepository<Employee>, IEmployeeRepository
    {

        public EmployeeRepository(POSContext context)
            : base(context)
        {
            
        }
    }
}
