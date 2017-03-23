using POS.Data.Repository.Repositories;
using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository
{
    public class PeopleRepository : UnDeleteableRepository<Person>, IPeopleRepository
    {
        public PeopleRepository(POSContext context)
            : base(context)
        {
        }

    }
}
