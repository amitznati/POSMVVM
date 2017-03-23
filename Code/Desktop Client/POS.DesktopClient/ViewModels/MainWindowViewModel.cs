using POS.Data.Repository;
using POS.DesktopClient.ViewModels.MainPos;
using POS.DesktopClient.ViewModels.Management;
using POS.DesktopClient.Views.General;
using POS.DesktopClient.Views.MainPos;
using POS.Windows;
using POSEntities.Entities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace POS.DesktopClient.ViewModels
{
    public class MainWindowViewModel: BindableViewModelBase
    {
        class FlyoutsNames
        {
            public const string MainPos = "MainPos";
            public const string Management = "Management";
            public const string EditPos = "EditPos";
        }
        public bool IsMainPosOpen
        {
            get {return GetValue(() => IsMainPosOpen); }
            set { SetValue(() => IsMainPosOpen, value); }
        }
        public bool IsManagementOpen
        {
            get { return GetValue(() => IsManagementOpen); }
            set { SetValue(() => IsManagementOpen, value); }
        }
        public bool IsEditPosOpen
        {
            get { return GetValue(() => IsEditPosOpen); }
            set { SetValue(() => IsEditPosOpen, value); }
        }
        public MainPOSViewModel MainPOSViewModel
        {
            get { return GetValue(() => MainPOSViewModel); }
            set { SetValue(() => MainPOSViewModel, value); }
        }
        public ManagementViewModel ManagementViewModel
        {
            get { return GetValue(() => ManagementViewModel); }
            set { SetValue(() => ManagementViewModel, value); }
        }
        public DelegateCommand<string> OpenFlyoutCommand { get; set; }
        public DelegateCommand TypeAttendanceCommand { get; set; }
        public DelegateCommand XReportCommand { get; set; }
        public DelegateCommand CloseDayCommand { get; set; }


        public MainWindowViewModel()
        {
            OpenFlyoutCommand = new DelegateCommand<string>(OpenFlyoutExecute);
            TypeAttendanceCommand = new DelegateCommand(TypeAttendanceExecute);
            MainPOSViewModel = new MainPOSViewModel();
            ManagementViewModel = new ManagementViewModel();
            XReportCommand = new DelegateCommand(XReportExecute);
            CloseDayCommand = new DelegateCommand(CloseDayExecute);
        }

        private void OpenFlyoutExecute(string url)
        {
            switch(url)
            {
                case FlyoutsNames.MainPos:
                    OpenMainPos();
                    break;
                case FlyoutsNames.Management:
                    IsManagementOpen = !IsManagementOpen;
                    break;
                case FlyoutsNames.EditPos:
                    IsEditPosOpen = !IsEditPosOpen;
                    break;
            }
        }

        private void OpenMainPos()
        {
            Employee emp = Dialogs.GetEmployeeInShift();
            if (emp != null)
            {
                MainPOSViewModel = new MainPOSViewModel();
                MainPOSViewModel.CurrentOrder.Employee = emp;
                IsMainPosOpen = true;
            }
            
        }
        private void TypeAttendanceExecute()
        {
            Dialogs.DailyAttendanceDialog();  
        }
        private void XReportExecute()
        {
            Employee emp = Dialogs.GetEmployee();
            if(emp!=null)
                Dialogs.XReportDialog(emp);
        }
        private void CloseDayExecute()
        {
            Employee emp = Dialogs.GetEmployee();
            if (emp != null)
                Dialogs.ZReportDialog(emp);
        }
    }
}
