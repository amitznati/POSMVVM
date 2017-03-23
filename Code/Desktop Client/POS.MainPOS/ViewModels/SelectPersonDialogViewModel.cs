using POS.Data.Repository;
using POS.Windows;
using POSEntities.Entities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POS.MainPOS.ViewModels
{
    public class SelectPersonDialogViewModel:BindableViewModelBase
    {
        public  Person SelectedObject
        {
            get { return GetValue(() => SelectedObject); }
            set { SetValue(() => SelectedObject, value); }
        }
        public CalculatorViewModel CalcVM { get; set; }
        public SelectPersonDialogViewModel():base()
        {
            CalcVM = new CalculatorViewModel();
        }
        public void SelectExecute()
        {
            SelectedObject = UnitOfWork.get().Peoples.Find(p => p.Password == CalcVM.TBText).FirstOrDefault();
        }
    }
}
