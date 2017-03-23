using Caliburn.Metro.Demo.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using POS.Data.Repository;
using POS.DesktopClient.Events.ManagmentEvents;
using POS.DesktopClient.Views.Managment;
using POS.Windows;
using POSEntities.Entities;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Windows;

namespace POS.DesktopClient.ViewModels.Management
{
    public class ManagementViewModel : BindableViewModelBase
    {
        private UnitOfWork unit;
        public DelegateCommand EditCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand NewCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        private IDialogCoordinator dialogCoordinator;
        private CustomDialogView CustomDialog;
        private Globals _globals;

        private IEventAggregator _eventAggregator;
        public ManagementViewModel()
        {
            _globals = Globals.get();
            _eventAggregator = _globals.EventAggregator;
            _eventAggregator.GetEvent<CancelDialog>().Subscribe(DialogCancelExecute);
            _eventAggregator.GetEvent<SelectedEntityChanged>().Subscribe(CanEditExecuteChanged);
            _eventAggregator.GetEvent<SaveDialogComplete>().Subscribe(DialogSaveExecute);
            DeleteCommand = new DelegateCommand(DeleteEntityExecute,CanDeleteEntityExecute);
          //  _eventAggregator.GetEvent<SaveDenied>().Subscribe(SaveDeniedExecute);

            unit = UnitOfWork.get();
            NewCommand = new DelegateCommand(NewExecute);
            EditCommand = new DelegateCommand(EditExecute, CanEditExecute);
            _globals.ActiveType = TabTypeMap[SelectedTabIndex];
            _globals.ActiveListNM = EntityListVMMap[SelectedTabIndex];
            dialogCoordinator = DialogCoordinator.Instance;
        }

        //private void SaveDeniedExecute(object entity)
        //{
        //    dialogCoordinator.ShowMessageAsync(this, Properties.Resources.SaveDeniedTitle, ((POSEntities.ModelBase)entity).ErrorMap.ToString(), new MessageDialogStyle() { }).ContinueWith(t => Console.WriteLine(t.Result));
        //}
        private void DeleteEntityExecute()
        {
            Globals.get().ActiveListNM.RemoveEntity(Globals.get().CurrentTypeForEdit);
        }
        private bool CanDeleteEntityExecute()
        {
            return Globals.get().CurrentTypeForEdit != null;
        }
        private void CanEditExecuteChanged(object obj)
        {
            EditCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }

        private void DialogCancelExecute(object obj)
        {
            _globals.IsInEditMode = false;
            _globals.IsInNewMode = false;
            dialogCoordinator.HideMetroDialogAsync(this, CustomDialog);
        }
        private void DialogSaveExecute(object dataContect)
        {
            _globals.IsInEditMode = false;
            _globals.IsInNewMode = false;
            dialogCoordinator.HideMetroDialogAsync(this, CustomDialog);
        }
        private void NewExecute()
        {
            _globals.IsInNewMode = true;
            CustomDialog = new CustomDialogView();
            dialogCoordinator.ShowMetroDialogAsync(this, CustomDialog);
        }

        private void EditExecute()
        {
            Globals.get().IsInEditMode = true;
            CustomDialog = new CustomDialogView();
            dialogCoordinator.ShowMetroDialogAsync(this, CustomDialog);
        }


        private bool CanEditExecute()
        {
            return Globals.get().CurrentTypeForEdit!=null;
        }

        private static Dictionary<int, Type> TabTypeMap = new Dictionary<int, Type>()
        {
            {0,typeof(Employee)},
            {1,typeof(Product)},
            {2,typeof(Customer)},
            {3,typeof(Vendor)},
            {4,typeof(Contact)},
            {5,typeof(Department)},
            {6,typeof(Group)},
            {7,typeof(Menu)}
            
        };

        public int SelectedTabIndex
        {
            get { return GetValue(() => SelectedTabIndex); }
            set { SetValue(() => SelectedTabIndex, value);
            Globals.get().ActiveType = TabTypeMap[SelectedTabIndex];
            Globals.get().ActiveListNM = EntityListVMMap[SelectedTabIndex];
            Globals.get().CurrentTypeForEdit = null;
            EditCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            }
        }

        private static Dictionary<int, IEntityListViewModelBase> _entityListVMMap = new Dictionary<int, IEntityListViewModelBase>()
        {
            {0,new EmployeeListViewModel()},
            {1,new ProductListViewModel()},
            {2,new CustomerListViewModel()},
            {3,new VendorListViewModel()},
            {4,new ContactListViewModel()},
            {5,new DepartmentListViewModel()},
            {6,new GroupListViewModel()}
        };

        public static Dictionary<int, IEntityListViewModelBase> EntityListVMMap
        {
            get { return _entityListVMMap; }
            set { _entityListVMMap = value; }
        }

    }
}
