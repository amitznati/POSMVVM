using POS.Data.Repository;
using POS.DesktopClient.Views.General;
using POS.DesktopClient.Views.MainPos;
using POS.Windows.Printer;
using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.DesktopClient
{
    public class Dialogs
    {
        public static Employee GetEmployee()
        {
            Person p = GetPerson();
            if (p != null && p.IsEmployee)
                return p as Employee;
            else
                Dialogs.ConfirmationDialog("עובד לא נמצא!");


            return null;
        }
        public static Person GetPerson()
        {
            SelectPersonDialog dialog = new SelectPersonDialog();
            if (dialog.ShowDialog() == true && dialog.SelectedPerson != null)
            {
                return dialog.SelectedPerson;
            }

            return null;
        }
        public static Employee GetEmployeeInShift()
        {
            SelectEmployeeInShiptDialog dialog = new SelectEmployeeInShiptDialog();
            if (dialog.ShowDialog() == true && dialog.SelectedEmployee != null)
            {
                return dialog.SelectedEmployee;
            }
            return null;
        }
        public static CashPayment GetCashPayment(double toPay)
        {
            CashDialog dialog = new CashDialog() { ToPay = toPay };
            if (dialog.ShowDialog() == true)
            {
                decimal amount = decimal.Parse((dialog.Payed ==0?toPay:dialog.Payed).ToString());
                decimal change = decimal.Parse(dialog.Change.ToString());
                CashPayment cp = new CashPayment()
                {
                    TotalPayed = amount - change,
                    RecievedAmount = amount,
                    Change = change
                };
                return cp;
            }
            return null;      
        }
        public static CreditPayment GetCreditPayment(double toPay)
        {
            CreditDialog dialog = new CreditDialog();
            if (dialog.ShowDialog() == true)
            {
                
            }
            return null;
        }

        public static Customer GetCustomer()
        {
            Person p = GetPerson();
            if(p!=null && p.IsCustomer)
            {
                return p as Customer;
            }
            else
                Dialogs.ConfirmationDialog("לקוח לא נמצא!");
            return null;
        }

        public static void XReportDialog( Employee emp)
        {
            XReportDialog dialog = new XReportDialog(ReportType.X,emp);
            if (dialog.ShowDialog() == true)
            {

            }
        }
        public static void ZReportDialog(Employee emp)
        {
            XReportDialog dialog = new XReportDialog(ReportType.Z, emp);
            if (dialog.ShowDialog() == true)
            {

            }
        }
        public static bool ConfirmationDialog(string message)
        {
            ConfirmationDialog cd = new ConfirmationDialog(message);
            return cd.ShowDialog() == true;
        }
        public static void DailyAttendanceDialog()
        {
            Employee emp = Dialogs.GetEmployee();
            if (emp != null)
            {
                DailyAttendance da = UnitOfWork.get().DailyAttendances.TypeAttendance(emp);
                PrinterAdapter.Instance.printDailyAttendances(da);
            }
        }

        public static Order GetSuspendedOrder(List<Order> orders)
        {
            SuspendedOrdersDialog dialog = new SuspendedOrdersDialog(orders);
            if(dialog.ShowDialog()==true)
            {
                return dialog.SelectedOrder;
            }
            return null;
        }

        
    }
}
