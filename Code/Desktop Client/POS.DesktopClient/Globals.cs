using POS.Data;
using POS.Data.Repository;
using POS.DesktopClient.ViewModels.Management;
using POSEntities.Entities;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POS.DesktopClient
{
    public class Globals
    {
        private static Globals _instance;
        private Globals() 
        {
            try
            {
                EventAggregator = new EventAggregator();
                CurrentCashier = new Repository<Cashier>(UnitOfWork.get().Context).Find(c => c.PCName.ToUpper() == Environment.MachineName).FirstOrDefault();
            }
            catch(Exception e)
            { throw e; }
        }
        public static Globals get()
        {
            if (_instance == null)
                _instance = new Globals();
            return _instance;
        }

        public Type ActiveType { get; set; }

        public object CurrentTypeForEdit { 
            get; 
            set; 
        }

        public bool IsInEditMode { get; set; }

        public bool IsInNewMode { get; set; }

        public IEventAggregator EventAggregator { get; set; }

        public IEntityListViewModelBase ActiveListNM { get; set; }

        public Cashier CurrentCashier { get; set; }

    }
}
