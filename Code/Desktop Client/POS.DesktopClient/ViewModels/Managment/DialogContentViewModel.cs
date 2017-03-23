using POS.Data.Repository;
using POS.DesktopClient.Events.ManagmentEvents;
using POS.Windows;
using POSEntities;
using POSEntities.Entities;
using Prism.Commands;
using Prism.Events;
using System;
using System.Windows;

namespace POS.DesktopClient.ViewModels.Management
{
    public class DialogContentViewModel:BindableViewModelBase
    {
        public FrameworkElement ContentView
        {
            get { return GetValue(() => ContentView); }
            set { SetValue(() => ContentView, value); }
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        private IEventAggregator _eventAggregator;
        private IEntityListViewModelBase EntityListNM;
        private ModelBase Model { get; set; }


        public DialogContentViewModel()
        {
            EntityListNM = Globals.get().ActiveListNM;
            setViewContent();
            _eventAggregator = Globals.get().EventAggregator;           
            Model = (ModelBase)Globals.get().CurrentTypeForEdit;
            Model.OnValidationComplete += Model_OnValidationComplete;
            
            SaveCommand = new DelegateCommand(SaveExecute,CanSaveDialogExecute);
            CancelCommand = new DelegateCommand(CancelExecute);
        }

        void Model_OnValidationComplete(object sender, EventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void CancelExecute()
        {
            _eventAggregator.GetEvent<CancelDialog>().Publish(Globals.get().CurrentTypeForEdit);
        }

        private void SaveExecute()
        {
            if (Model is Person)
            {
               if(UnitOfWork.get().Peoples.Find(p => p.Password == ((Person)Model).Password).Count>0)
               {
                   MessageBox.Show("לא ניתן להשתמש בסיסמא שנבחרה!");
                   return;
               }
            }
            EntityListNM.SaveNewEntity(Model);
        }

        private bool CanSaveDialogExecute()
        {
            
            return Model.ErrorMap.Count == 0;
        }

        private void setViewContent()
        {
            ContentView = EntityListNM.getEntityView();
        }

    }
}
