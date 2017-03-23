using POS.Data.Repository.Core.Repository;
using POSEntities.Entities;

namespace POS.Data.Repository
{
    public interface IProductRepository : IUnDeleteableRepository<Product>
    {
    }
}
