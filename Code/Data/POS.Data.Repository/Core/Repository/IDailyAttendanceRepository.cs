using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository.Core.Repository
{
    public interface IDailyAttendanceRepository :IRepository<DailyAttendance>
    {
        List<DailyAttendance> GetAllInShifp();
        DailyAttendance TypeAttendance(Employee emp);
    }
}
