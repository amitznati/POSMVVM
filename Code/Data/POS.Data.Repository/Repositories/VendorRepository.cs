using POS.Data.Repository.Core.Repository;
using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository.Repositories
{
    public class VendorRepository : UnDeleteableRepository<Vendor>, IVendorRepository
    {
        public VendorRepository(POSContext context) : base(context) { }
    }
}
