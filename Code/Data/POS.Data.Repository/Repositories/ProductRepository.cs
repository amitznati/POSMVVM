using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository.Repositories
{
    public class ProductRepository : UnDeleteableRepository<Product>, IProductRepository
    {
        public ProductRepository(POSContext context) : base(context)
        { }
    }
}
