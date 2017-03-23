using MahApps.Metro.Controls.Dialogs;
using POS.Data.Repository;
using POS.DesktopClient.Events.MainPosEvents;
using POS.DesktopClient.Views.MainPos;
using POS.Windows;
using POS.Windows.Printer;
using POSEntities.Entities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace POS.DesktopClient.ViewModels.MainPos
{
    public class MainPOSViewModel:BindableViewModelBase
    {
        private IDialogCoordinator dialogCoordinator;
        public Prism.Events.IEventAggregator _eventAggregator { get; set; }
        public List<Payment> PartialPayments
        {
            get { return GetValue(() => PartialPayments); }
            set { SetValue(() => PartialPayments, value); }
        }
        public bool IsPartialPayed
        {
            get { return GetValue(() => IsPartialPayed); }
            set { SetValue(() => IsPartialPayed, value); }
        }
        public decimal LeftToPay
        {
            get { return GetValue(() => LeftToPay); }
            set { SetValue(() => LeftToPay, value);
            if (LeftToPay == CurrentOrder.TotalPayment||LeftToPay==0) IsPartialPayed = false;
            else IsPartialPayed = true;
            }
        }
        public bool IsMoreOptionOpen
        {
            get { return GetValue(() => IsMoreOptionOpen); }
            set { SetValue(() => IsMoreOptionOpen, value); }
        }
        public Menu CurrentMenu
        {
            get { return GetValue(() => CurrentMenu); }
            set { SetValue(() => CurrentMenu, value); }
        }
        public Order CurrentOrder
        {
            get { return GetValue(() => CurrentOrder); }
            set { SetValue(() => CurrentOrder, value); }
        }
        public OrderLine SelectedRow
        {
            get { return GetValue(() => SelectedRow); }
            set { SetValue(() => SelectedRow, value); DeleteRowCommand.RaiseCanExecuteChanged(); }
        }
        public List<Order> SuspendedOrders { get; set; }
        public int SuspendedOrdersCount
        {
            get { return GetValue(() => SuspendedOrdersCount); }
            set { SetValue(() => SuspendedOrdersCount, value); IsAnySuspendedOrders = SuspendedOrdersCount > 0; }
        }
        public bool IsAnySuspendedOrders
        {
            get { return GetValue(() => IsAnySuspendedOrders); }
            set { SetValue(() => IsAnySuspendedOrders, value); }
        }
        public bool IsSearchOrderOpen
        {
            get { return GetValue(() => IsSearchOrderOpen); }
            set { SetValue(() => IsSearchOrderOpen, value); }
        }
        public DelegateCommand CashCommand { get; set; }
        public DelegateCommand CreditCommand { get; set; }
        public DelegateCommand SelectCustomerCommand { get; set; }
        public DelegateCommand OpenDrawerCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }
        public DelegateCommand SuspendOrderCommand { get; set; }
        public DelegateCommand SuspendedOrdersCommand { get; set; }
        public DelegateCommand SearchOrderCommand { get; set; }
        public DelegateCommand DeleteRowCommand { get; set; }
        public DelegateCommand ClearCommand { get; set; }
        public DelegateCommand MoreOptionCommand { get; set; }
        public DelegateCommand<Payment> CancelPaymentCommand { get; set; }

        public CalculatorViewModel CalcVM { get { return GetValue(() => CalcVM); } set { SetValue(() => CalcVM, value); } }
        public MainPOSViewModel()
        {
            CashCommand = new DelegateCommand(OpenCashDialog);
            CreditCommand = new DelegateCommand(OpenCreditDialog);
            SelectCustomerCommand = new DelegateCommand(OpenCustomerDialog);
            OpenDrawerCommand = new DelegateCommand(OpenDrawer);
            ExitCommand = new DelegateCommand(ExitExecute);
            SuspendOrderCommand = new DelegateCommand(SuspendOrder);
            SuspendedOrdersCommand = new DelegateCommand(OpenSuspendedOrdersDialog);
            SearchOrderCommand = new DelegateCommand(SearchOrderDialog);
            DeleteRowCommand = new DelegateCommand(DeleteRow, CanDeleteRow);
            ClearCommand = new DelegateCommand(ClearOrder);
            MoreOptionCommand = new DelegateCommand(MoreOptionExecute);
            CancelPaymentCommand = new DelegateCommand<Payment>(CancelPayment);
            _eventAggregator = Globals.get().EventAggregator;
            _eventAggregator.GetEvent<POSItemClick>().Subscribe(POSItemClickExecute);
            initOrder();
            CurrentMenu = UnitOfWork.get().Menus.Get(1);
            CalcVM = new CalculatorViewModel();
            dialogCoordinator = DialogCoordinator.Instance;
            SuspendedOrders = new List<Order>();

        }

        public void MoreOptionExecute()
        {
            IsMoreOptionOpen = !IsMoreOptionOpen;
        }
        private void CancelPayment(Payment orderPayment)
        {
            LeftToPay += orderPayment.TotalPayed;
            PartialPayments.Remove(orderPayment);
            CurrentOrder.Payments = new HashSet<Payment>(PartialPayments);
        }
        private void POSItemClickExecute(ItemDisplayInfo item)
        {
            if (item.IsMenu)
                CurrentMenu = item.Menus.FirstOrDefault();
            else
            {
                Product prod = item.Products.FirstOrDefault();
                OrderLine line = CurrentOrder.OrderLines.Where(c => c.prodId == prod.ID).FirstOrDefault();
                if (line == null)
                    line = AddItemToOrder(prod);
                else
                    line = UpdateItemInOrder(prod);
                CollectionViewSource.GetDefaultView(CurrentOrder.OrderLines).Refresh();
                decimal addedAmount = prod.salePrice * (decimal)getQuantity();
                CurrentOrder.TotalPayment += addedAmount;
                LeftToPay += addedAmount;
                CalcVM.TBText = String.Empty;
            }

        }
        private OrderLine UpdateItemInOrder(Product prod)
        {
            OrderLine line = CurrentOrder.OrderLines.Where(l => l.prodId == prod.ID).FirstOrDefault();
            line.quantity += getQuantity();
            line.prodTotalPrice = (decimal)line.quantity * line.prodUnitPrice;
            return line;
        }


        private OrderLine AddItemToOrder(Product prod)
        {
            OrderLine newLine = new OrderLine()
                {
                    prodId = prod.ID,
                    Product = prod,
                    quantity = getQuantity(),
                    prodUnitPrice = prod.salePrice
                };
            newLine.prodTotalPrice = newLine.prodUnitPrice * (decimal)newLine.quantity;
            CurrentOrder.OrderLines.Add(newLine);
            return newLine;
        }
        private double getQuantity()
        {
            double count = 1;
            double.TryParse(CalcVM.TBText, out count);
            return Math.Max(1, count);
        }
        private void OpenCashDialog()
        {
            CashPayment cp = Dialogs.GetCashPayment(double.Parse(LeftToPay.ToString()));
            if (cp != null)
            {
                PartialPayments.Add(cp);
                CurrentOrder.Payments = new HashSet<Payment>(PartialPayments);
                LeftToPay -= cp.TotalPayed;
                if (LeftToPay <= 0)
                   CloseOrder();
            }
            
        }

        private void CloseOrder()
        {
            CurrentOrder.Payments = PartialPayments;
            UnitOfWork.get().Orders.SaveOrder(CurrentOrder);
            PrinterAdapter.Instance.printRecipte(CurrentOrder);
            initOrder();
        }
        private void OpenCreditDialog()
        {
            CreditPayment cp = Dialogs.GetCreditPayment(double.Parse(LeftToPay.ToString()));
            if(cp!=null)
            {

            }
        }
        private void OpenCustomerDialog()
        {
            CurrentOrder.Customer = Dialogs.GetCustomer();
        }
        private void OpenDrawer()
        {
            RawPrinterHelper.OpenCashDrew();
        }
        private void ExitExecute()
        {

        }
        private void SuspendOrder()
        {
            SuspendedOrders.Add(CurrentOrder);
            SuspendedOrdersCount = SuspendedOrders.Count;
            initOrder();
        }
        private void OpenSuspendedOrdersDialog()
        {
            Order order = Dialogs.GetSuspendedOrder(SuspendedOrders);
            if(order != null)
            {
                CurrentOrder = order;
                SuspendedOrders.Remove(order);
                SuspendedOrdersCount = SuspendedOrders.Count;
            }
        }
        private void SearchOrderDialog()
        {
            IsSearchOrderOpen = true;
        }
        private void DeleteRow()
        {
            if (SelectedRow != null)
            {
                CurrentOrder.OrderLines.Remove(SelectedRow);
                CurrentOrder.TotalPayment -= SelectedRow.prodTotalPrice;
                LeftToPay -= SelectedRow.prodTotalPrice;
                CollectionViewSource.GetDefaultView(CurrentOrder.OrderLines).Refresh();
            }
        }
        private bool CanDeleteRow() { return SelectedRow != null; }
        private void ClearOrder()
        {
            CurrentOrder.OrderLines = new HashSet<OrderLine>();
            CollectionViewSource.GetDefaultView(CurrentOrder.OrderLines).Refresh();
        }

        private void initOrder()
        {
            int currentEmpID = CurrentOrder !=null && CurrentOrder.EmployeeID != 0 ? CurrentOrder.EmployeeID : 1;
            CurrentOrder = new Order() { 
                Tax = UnitOfWork.get().Taxes.GetAll().Where(t => t.Mode = true).FirstOrDefault() ,
                Customer = UnitOfWork.get().Customers.AnonymousCustomer,
                Employee = UnitOfWork.get().Employees.Get(currentEmpID),
                PurchaseDate = DateTime.Now,
                Cashier = Globals.get().CurrentCashier,
                StoreInfo = Globals.get().CurrentCashier.StoreInfo
            };
            LeftToPay = 0;
            PartialPayments = new List<Payment>();
        }

        private void CopyOrderFromSearch(Order order)
        {
            CurrentOrder.Customer = order.Customer;
            CurrentOrder.OrderLines = order.OrderLines;

        }


    }
}
