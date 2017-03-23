using POS.Data.Repository;
using POS.DesktopClient.Views.General;
using POS.Windows;
using POS.Windows.Printer;
using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace POS.DesktopClient.ViewModels.General
{
   
    public class XReportDialogViewModel : BindableViewModelBase
    {
        public XReportHolder XReport { get; set; }
        public string ReportName { get; set; }
        public bool IsXMode { get; set; }
        public XReportDialogViewModel(ReportType type)
        {
            XReport = new XReportHolder(UnitOfWork.get().Orders.GetAllOpenInZ());
            IsXMode = type.Value.Equals(ReportType.X.Value);
            ReportName = type.Value;
        }

        internal void CloseDay()
        {
            ZReport report = new ZReport()
            {
                Employee = XReport.Employee = UnitOfWork.get().Employees.Get(1),
                Orders = XReport.Orders,
                reportDate = DateTime.Now,
                reprotTotalAmount = decimal.Parse(XReport.TotalAmountForReport.ToString())
            };
            XReport.ZReport = report;
            UnitOfWork.get().Context.ZReports.Add(report);
            this.XReport.Orders.Select(o => { o.IsCloseInZ = true; return o; }).ToList();
            UnitOfWork.get().Complete();
            PrinterAdapter.Instance.PrintZReport(XReport);
        }

        internal void PrintReport()
        {
            switch(ReportName)
            {
                case "X":
                    PrinterAdapter.Instance.PrintXreport(XReport);
                    break;
                case "Z":
                    PrinterAdapter.Instance.PrintZReport(XReport);
                    break;
            }
        }
    }
}
