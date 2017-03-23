using POS.Data.Repository.Core.Repository;
using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository.Repositories
{
    public class GroupRepository : UnDeleteableRepository<Group>, IGroupRepository
    {
        public GroupRepository(POSContext context) : base(context) { }
    }
}
