using MahApps.Metro.Controls;
using POS.DesktopClient.ViewModels.General;
using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace POS.DesktopClient.Views.General
{
    /// <summary>
    /// Interaction logic for XReportDialog.xaml
    /// </summary>
    public partial class XReportDialog : MetroWindow
    {
        ReportType reportType;
        XReportDialogViewModel viewModel;
        
        public XReportDialog(ReportType type,Employee emp)
        {
            reportType = type;
            viewModel = new XReportDialogViewModel(type);
            viewModel.XReport.Employee = emp;
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (reportType.Value.Equals(General.ReportType.Z.Value))
                viewModel.CloseDay();
            this.DialogResult = true;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            viewModel.PrintReport();
            this.DialogResult = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
    public class ReportType
    {
        private ReportType(string name) { Value = name; }
        public string Value { get; set; }
        public static ReportType X { get { return new ReportType("X"); } }
        public static ReportType Z { get { return new ReportType("Z"); } }
    }
}
