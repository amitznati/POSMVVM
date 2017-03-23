using MahApps.Metro.Controls.Dialogs;
using POSEntities;
using POSEntities.Entities;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace POS_WPF.ViewModels
{
    class MainWindowViewModel : BindableViewModelBase
    {

        public bool IsDataListOpen
        {
            get { return GetValue(()=>IsDataListOpen); }
            set { SetValue(() => IsDataListOpen, value); }
        }
        public bool IsPOSOpen
        {
            get { return GetValue(() => IsPOSOpen); }
            set { SetValue(() => IsPOSOpen, value); }
        }
        //private readonly IRegionManager _regionManager;
        public DelegateCommand<string> NavigateCommand { get; set; }

        public string MAIN_POS { get { return "0"; } }
        public string EMPLOYEES { get { return "1"; } }
        public MainWindowViewModel(/*IRegionManager regionManager*/)
        {
            
          //  _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        public void Navigate(string fnum)
        {
            int num = Int32.Parse(fnum);
            switch(num)
            {
                case 0:
                    IsPOSOpen = !IsPOSOpen;
                    break;
                case 1:
                    IsDataListOpen = !IsDataListOpen;
                    break;

            }
        }



    }
}
