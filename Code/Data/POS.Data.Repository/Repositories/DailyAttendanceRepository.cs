using POS.Data.Repository.Core.Repository;
using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository.Repositories
{
    public class DailyAttendanceRepository : Repository<DailyAttendance>, IDailyAttendanceRepository
    {

        public DailyAttendanceRepository(POSContext context):base(context)
        {
            _context = context;
        }
        public List<DailyAttendance> GetAllInShifp()
        {
            List<DailyAttendance> retVal = new List<DailyAttendance>();
            var all = this.GetAll();
            if (all.Count() == 0) return retVal;
            var allInShipt = all.Where(e => e.isPresent == true);
            if (allInShipt.Count() == 0) return retVal;
            return allInShipt.ToList<DailyAttendance>();
        }

        private DailyAttendance ExitShipf(Employee emp)
        {
            var da = this.Find(e => e.empId == emp.ID && e.isPresent == true).First();
            da.isPresent = false;
            da.exitTime = DateTime.Now;
            return da;
        }

        private DailyAttendance EnterShipf(Employee emp)
        {
            var da = new DailyAttendance()
            {
                empId = emp.ID,
                entryTime = DateTime.Now,
                isPresent = true
            };
            this.Add(da);
            return da;
        }

        private POSContext _context { get; set; }


        public DailyAttendance TypeAttendance(Employee emp)
        {
            DailyAttendance retDA;
            var allInShift = GetAllInShifp();
            if (allInShift.Count == 0 || allInShift.Where(e => e.empId == emp.ID).Count() == 0)
            {
                retDA = EnterShipf(emp);
            }
            else retDA = ExitShipf(emp);
            this.Context.SaveChanges();
            return retDA;
        }
    }
}
