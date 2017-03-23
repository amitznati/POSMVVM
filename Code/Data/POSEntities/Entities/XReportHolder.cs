using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSEntities.Entities
{
    public class XReportHolder
    {
        public ZReport ZReport { get; set; }
        public XReportHolder(List<Order> orders)
        {
            DepartmentsStatistic = new List<DepartmentStatistic>();
            PaymentsStatistic = new List<Statistic>();
            EmployeeStatistic = new List<Statistic>();
            Orders = orders;
            foreach (Order order in Orders)
            {
                foreach (Payment p in order.Payments)
                    AddPayment(p);
                foreach (OrderLine line in order.OrderLines)
                    AddDepartmentStatistic(line);
                AddEmployeeStatistic(order);
            }
            calcPercentage();
        }

        private void AddEmployeeStatistic(Order order)
        {
            Statistic es = null;
            if (EmployeeStatistic.Count > 0 && EmployeeStatistic.Where(e => e.ID == order.Employee.ID).Count() > 0)
                es = EmployeeStatistic.Where(e => e.ID == order.Employee.ID).First();
            if(es==null)
            {
                es = new Statistic()
                {
                    Name = order.Employee.FullName,
                    ID = order.Employee.ID,
                    Count = 1,
                    TotalAmount = double.Parse(order.TotalPayment.ToString())
                };
                EmployeeStatistic.Add(es);
            }
            else
            {
                es.Count++;
                es.TotalAmount += double.Parse(order.TotalPayment.ToString());
            }
        }
        public List<Statistic> EmployeeStatistic { get; set; }
        public List<Statistic> PaymentsStatistic { get; set; }
        public List<DepartmentStatistic> DepartmentsStatistic { get; set; }
        public List<Order> Orders { get; set; }
        private void calcPercentage()
        {
            foreach(DepartmentStatistic ds in DepartmentsStatistic)
            {
                ds.Percentage = ds.TotalAmount / TotalAmountForReport * 100;
                foreach(Statistic gs in ds.GroupStatistic)
                {
                    gs.Percentage = gs.TotalAmount / TotalAmountForReport * 100;
                }
            }
            foreach(Statistic ps in PaymentsStatistic)
            {
                ps.Percentage = ps.TotalAmount / TotalAmountForReport * 100;
            }
            foreach(Statistic es in EmployeeStatistic)
            {
                es.Percentage = es.TotalAmount / TotalAmountForReport * 100;
            }
        }
        public Employee Employee { get; set; }   
        public void AddDepartmentStatistic(OrderLine line)
        {
            Group group = line.Product.Group;

            DepartmentStatistic ds = null;
            
            if (DepartmentsStatistic.Count > 0 && DepartmentsStatistic.Where(g => g.Name == group.Department.DeptName).Count() > 0)
                ds = DepartmentsStatistic.Where(g => g.Name == group.Department.DeptName).First();
            if (ds == null)
            {
                ds = new DepartmentStatistic()
                {
                    Name = group.Department.DeptName
                };
                ds.AddGroupStatistic(line);
                DepartmentsStatistic.Add(ds);
            }
            else
                ds.AddGroupStatistic(line);
        }
        public void AddPayment(Payment p)
        {
            Statistic ps = null;
            if (PaymentsStatistic.Count > 0 && PaymentsStatistic.Where(pa => pa.Name.Equals(p.PaymentTypeName)).Count() > 0)
                ps = PaymentsStatistic.Where(pa => pa.Name.Equals(p.PaymentTypeName)).First();
            if (ps == null)
            {
                ps = new Statistic()
                {
                    Name = p.PaymentTypeName,
                    TotalAmount = double.Parse(p.TotalPayed.ToString()),
                    Count = 1
                };
                PaymentsStatistic.Add(ps);
            }
            else
            {
                ps.Count++;
                ps.TotalAmount += double.Parse(p.TotalPayed.ToString());
            }

            TotalAmountForReport += double.Parse(p.TotalPayed.ToString());
        }
        public double TotalAmountForReport { get; set; }
       
    }
    public class DepartmentStatistic:Statistic
    {
        public DepartmentStatistic()
        {
            GroupStatistic = new List<Statistic>();
        }
        public List<Statistic> GroupStatistic { get; set; }
        public void AddGroupStatistic(OrderLine line)
        {
            Group group = line.Product.Group;
            double amount = double.Parse(line.prodTotalPrice.ToString());
            Statistic gs = null;
            if (GroupStatistic.Count > 0 && GroupStatistic.Where(g => g.Name == group.GroupName).Count() > 0)
                gs = GroupStatistic.Where(g => g.Name == group.GroupName).First();
            if (gs == null)
            {
                gs = new Statistic()
                {
                    Name = group.GroupName,
                    TotalAmount = amount
                };
                GroupStatistic.Add(gs);
            }
            else
                gs.TotalAmount += amount;
            TotalAmount += amount;
            gs.Count += line.quantity;
            Count += line.quantity;
        }
    }

    public class Statistic
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Count{get;set;}
        public double TotalAmount{get;set;}
        public double Percentage {get;set;}
    }
}
