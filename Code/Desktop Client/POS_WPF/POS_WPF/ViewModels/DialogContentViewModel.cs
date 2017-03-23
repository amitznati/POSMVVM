using POS_WPF.Views;
using POSEntities;
using POSEntities.Entities;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POS_WPF.ViewModels
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
            _eventAggregator.GetEvent<Events.CancelDialog>().Publish(Globals.get().CurrentTypeForEdit);
        }

        private void SaveExecute()
        {
            if (Model.ErrorMap.Count == 0)
                EntityListNM.SaveNewEntity(Model);
            else
            {
                _eventAggregator.GetEvent<Events.SaveDenied>().Publish(Globals.get().CurrentTypeForEdit);
            }
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
