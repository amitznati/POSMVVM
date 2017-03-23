using POS.Data.Repository.Core.Repository;
using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository.Repositories
{
    public class RoleRepository: Repository<Role>, IRoleRepository
    {
        public RoleRepository(POSContext context) : base(context) { }
    }
}
