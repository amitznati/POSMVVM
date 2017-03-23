using POS.Data.Repository.Core.Repository;
using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository.Repositories
{
    public class OrderRepository:Repository<Order>,IOrderRepository
    {
        POSContext _context;
        public OrderRepository(POSContext context):base(context)
        {
            _context = context;
        }

        public int GetDailyNum()
        {
            int num = 1;
            List<Order> orders = this.Find(o => o.IsCloseInZ == false);
            if (orders.Count() == 0) return num;
            num = orders.OrderBy(o => o.DailyNumber).Last().DailyNumber + 1;
            return num;
        }

        private static object syncRoot = new Object();

        public void SaveOrder(Order order)
        {
            lock (syncRoot)
            {
                try
                {
                    int id;
                    order.DailyNumber = GetDailyNum();
                    this.Add(order);
                    id = _context.SaveChanges();
                    
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }


        public List<Order> GetAllOpenInZ()
        {
            return this.Find(o => o.IsCloseInZ == false);
        }
    }
}
