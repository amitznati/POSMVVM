using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using POS.Data.Repository;
using POS.DesktopClient.Events.MainPosEvents;
using POS.DesktopClient.Views.MainPos;
using POS.Windows;
using POSEntities;
using POSEntities.Entities;
using Prism.Commands;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Media.Imaging;


namespace POS.DesktopClient.ViewModels.MainPos
{
    public class EditPOSMenuViewModel:BindableViewModelBase
    {
        private IDialogCoordinator dialogCoordinator;
        private CustomDialog CustomDialog;
        public DelegateCommand SelectItemCommand { get; set; }
        public DelegateCommand AddItemCommand { get; set; }
        public DelegateCommand SelectFontColorCommand { get; set; }
        public DelegateCommand SelectColorCommand { get; set; }
        public DelegateCommand SelectImageCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public EditPOSMenuViewModel()
        {
            _eventAggregator = Globals.get().EventAggregator;
            _eventAggregator.GetEvent<SelectItemComplete>().Subscribe(SelectItemComplete);
            _eventAggregator.GetEvent<SelectItemCanceled>().Subscribe(SelectItemCanceled);
            _eventAggregator.GetEvent<DisplayItemEditClick>().Subscribe(DisplayItemEditClickExecute);
            dialogCoordinator = DialogCoordinator.Instance;
            SelectItemCommand = new DelegateCommand(SelectItemExecute);
            AddItemCommand = new DelegateCommand(AddItemExecute);
            SelectFontColorCommand = new DelegateCommand(SelectFontColorExecute);
            SelectColorCommand = new DelegateCommand(SelectColorExecute);
            SelectImageCommand = new DelegateCommand(SelectImageExecute);
            SaveCommand = new DelegateCommand(SaveExecute);
        }
        private void DisplayItemEditClickExecute(ItemDisplayInfo item)
        {
            this.CurrentItemInDesign = item;
            CanEditItem = true;
        }
        private void SaveExecute()
        {
            try
            {
                UnitOfWork.get().Complete();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SelectColorExecute()
        {
            CustomColorDialog dialog = new CustomColorDialog();
            if (dialog.ShowDialog()==true)
            {
                CurrentItemInDesign.BackgroundColor = dialog.Color;
            }        
        }
        private void SelectImageExecute()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                CurrentItemInDesign.ImageUrl =  op.FileName;
            }
        }
        private void SelectFontColorExecute()
        {
            CustomColorDialog dialog = new CustomColorDialog();
            if (dialog.ShowDialog() == true)
            {
                CurrentItemInDesign.TextColor = dialog.Color;
            }
        }

        private void AddItemExecute()
        {
            _eventAggregator.GetEvent<AddItemEvent>().Publish(CurrentItemInDesign);
            CurrentItemInDesign = null;
            CanEditItem = false;
        }
        private void SelectItemComplete(IDisplayableModel model)
        {
            dialogCoordinator.HideMetroDialogAsync(this, CustomDialog);
            CurrentItemInDesign =  model.GetNewDiplayInfo();
            CanEditItem = true;
        }
        private void SelectItemCanceled(object obj)
        {
            dialogCoordinator.HideMetroDialogAsync(this, CustomDialog);
        }
        
        public ItemDisplayInfo CurrentItemInDesign
        {
            get { return GetValue(() => CurrentItemInDesign); }
            set { SetValue(() => CurrentItemInDesign, value); }
        }
        private void SelectItemExecute()
        {
            SelectDiaplyableItemDialog dialog = new SelectDiaplyableItemDialog();
            if(dialog.ShowDialog() == true)
            {
                CurrentItemInDesign = ((IDisplayableModel)dialog.SelectedItem).GetNewDiplayInfo();
                CanEditItem = true; 
            }
            //CustomDialog = new CustomDialogView() { DataContext = new CustomDialogViewModel() };
           // dialogCoordinator.ShowMetroDialogAsync(this, CustomDialog);
        }
        public bool CanEditItem
        {
            get { return GetValue(() => CanEditItem); }
            set { SetValue(() => CanEditItem, value); }
        }
        public IEventAggregator _eventAggregator { get; set; }
    }
}
