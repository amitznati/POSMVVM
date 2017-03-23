using POS.Data.Repository;
using POS.DesktopClient.Events.MainPosEvents;
using POS.Windows;
using POS.Windows.Printer;
using POSEntities.Entities;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.DesktopClient.ViewModels.MainPos
{
    public class SearchOrderDialogViewModel:BindableViewModelBase
    {
        public List<Order> Orders
        {
            get { return GetValue(() => Orders); }
            set { SetValue(() => Orders, value); }
        }
        public Order SelectedOrder
        {
            get { return GetValue(() => SelectedOrder); }
            set { 
                SetValue(() => SelectedOrder, value);
                isSearchCommandsEnables = SelectedOrder != null;
            }
        }
        public List<Department> Departments
        {
            get { return GetValue(() => Departments); }
            set { SetValue(() => Departments, value); }
        }
        public List<Department> SelectedDepartments
        {
            get { return GetValue(() => SelectedDepartments); }
            set{SetValue(() => SelectedDepartments, value);}
        }
        public List<Group> Groups
        {
            get { return GetValue(() => Groups); }
            set { SetValue(() => Groups, value); }
        }
        public List<Group> SelectedGroups
        {
            get { return GetValue(() => SelectedGroups); }
            set{SetValue(() => SelectedGroups, value);}
        }
        public List<Product> Products
        {
            get { return GetValue(() => Products); }
            set { SetValue(() => Products, value); }
        }
        public List<Product> SelectedProducts
        {
            get { return GetValue(() => SelectedProducts); }
            set { SetValue(() => SelectedProducts, value); }
        }
        public DateTime DateFrom
        {
            get { return GetValue(() => DateFrom); }
            set { SetValue(() => DateFrom, value); }
        }
        public DateTime DateTo
        {
            get { return GetValue(() => DateTo); }
            set { SetValue(() => DateTo, value); }
        }
        public List<string> PaymentsType
        {
            get { return GetValue(() => PaymentsType); }
            set { SetValue(() => PaymentsType, value); }
        }
        public List<string> SelectedPaymentsType
        {
            get { return GetValue(() => SelectedPaymentsType); }
            set { SetValue(() => SelectedPaymentsType, value); }
        }
        public List<bool> IsCloseInZ
        {
            get { return GetValue(() => IsCloseInZ); }
            set { SetValue(() => IsCloseInZ, value); }
        }
        public List<string> CloseInZList
        {
            get { return GetValue(() => CloseInZList); }
            set { SetValue(() => CloseInZList, value); }
        }
        public List<string> SelectedCloseInZList
        {
            get { return GetValue(() => SelectedCloseInZList); }
            set { SetValue(() => SelectedCloseInZList, value); }
        }
        public int NumberOfPages
        {
            get { return GetValue(() => NumberOfPages); }
            set { SetValue(() => NumberOfPages, value); }
        }
        public int PageNumber
        {
            get { return GetValue(() => PageNumber); }
            set { SetValue(() => PageNumber, value); }
        }
        public int PageSize
        {
            get { return GetValue(() => PageSize); }
            set { SetValue(() => PageSize, value); }
        }
        public int ResultCount
        {
            get { return GetValue(() => ResultCount); }
            set { SetValue(() => ResultCount, value); }
        }
        public int CountFrom
        {
            get { return GetValue(() => CountFrom); }
            set { SetValue(() => CountFrom, value); }
        }
        public int CountTo
        {
            get { return GetValue(() => CountTo); }
            set { SetValue(() => CountTo, value); }
        }
        public bool isSearchCommandsEnables
        {
            get { return GetValue(() => isSearchCommandsEnables); }
            set { SetValue(() => isSearchCommandsEnables, value); }
        }

    
        private IEventAggregator eventAggregator;
        public DelegateCommand FilterCommand { get; set; }
        private IEnumerable<Order> AllOrder;
        public DelegateCommand NextCommand { get; set; }
        public DelegateCommand PrevCommand { get; set; }
        public DelegateCommand FullRefundCommand { get; set; }
        public DelegateCommand PartialRefundCommand { get; set; }
        public DelegateCommand PrintReceiptCommand { get; set; }
        public DelegateCommand CopyOrderCommand { get; set; }
        public SearchOrderDialogViewModel()
        {
            eventAggregator = Globals.get().EventAggregator;
            PageSize = 20;
            
            Products = new List<Product>();
            Groups = new List<Group>();
            
            Departments = UnitOfWork.get().Departments.GetAll().ToList();
            SelectedDepartments = new List<Department>(Departments);
            RefreshDepartment();
            DateFrom = DateTime.Now.AddYears(-1);
            DateTo = DateTime.Now;
            PaymentsType = new List<string>() { "מזומן", "אשראי" };
            SelectedPaymentsType = new List<string>(PaymentsType);
            CloseInZList = new List<string>() { "פתוחה", "סגורה" };
            SelectedCloseInZList = new List<string>(CloseInZList);
            FilterCommand = new DelegateCommand(FilterExecute);
            NextCommand = new DelegateCommand(NextPage);
            PrevCommand = new DelegateCommand(PrevPage);
            CopyOrderCommand = new DelegateCommand(() => { eventAggregator.GetEvent<CopyOrderFromSearch>().Publish(SelectedOrder); });
            PrintReceiptCommand = new DelegateCommand(() => { PrinterAdapter.Instance.printRecipte(SelectedOrder); });
            FullRefundCommand = new DelegateCommand(() => { eventAggregator.GetEvent<FullRefundFromSearch>().Publish(SelectedOrder); });
            PartialRefundCommand = new DelegateCommand(() => { eventAggregator.GetEvent<PartialRefundFromSearch>().Publish(SelectedOrder); });
        }
        public void RefreshDepartment()
        {
            var groups = new List<Group>();
            var products = new List<Product>();
            foreach (Department d in SelectedDepartments)
            {
                groups.AddRange(d.Groups);
                products.AddRange(d.Products);
            }
            Groups = new List<Group>(groups);
            Products = new List<Product>(products);
            SelectedProducts = new List<Product>(products);
            SelectedGroups = new List<Group>(groups);
        }
        public void RefreshGroups()
        {
            var products = new List<Product>();
            foreach (Group g in SelectedGroups)
            {  
                products.AddRange(g.Products);
                
            }
            Products = new List<Product>(products);
            SelectedProducts = new List<Product>(Products);
        }

        
        private void FilterExecute()
        {
            PageNumber = 1;
            AllOrder = UnitOfWork.get().Orders.GetAll().Where(o =>
                o.OrderLines.Any(ol => SelectedProducts.Contains(ol.Product)) &&
                o.Payments.Any(p => SelectedPaymentsType.Contains(p.PaymentTypeName)) &&
                o.PurchaseDate >= DateFrom && o.PurchaseDate <= DateTo &&
                (o.IsCloseInZ == SelectedCloseInZList.Contains("פתוחה") ||
                o.IsCloseInZ != SelectedCloseInZList.Contains("סגורה"))
                ).OrderByDescending(o => o.ID);
            ResultCount = AllOrder.Count();
            NumberOfPages = ResultCount/PageSize+
                (ResultCount%PageSize==0?0:1);
            
            RefreshOrders();
        }
        private void NextPage()
        {
            if (PageNumber < NumberOfPages)
            {
                PageNumber++;
                RefreshOrders();
            }
        }
        private void PrevPage()
        {
            if (PageNumber > 1)
            {
                PageNumber--;
                RefreshOrders();
            }
        }
        private void RefreshOrders()
        {
            Orders = new List<Order>(AllOrder.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList());
            CountFrom = (PageNumber - 1) * PageSize;
            CountTo = CountFrom + Orders.Count;
        }

        private void copyOrderExecute()
        {
            
        }

    }
}
