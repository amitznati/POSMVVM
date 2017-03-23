using POS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POS_WPF
{
    public class Globals
    {
        private static Globals _instance;
        private Globals() 
        {
        }
        public static Globals get()
        {
            if (_instance == null)
                _instance = new Globals();
            return _instance;
        }

        public Type ActiveType { get; set; }

        public object CurrentTypeForEdit { get; set; }

        public bool IsInEditMode { get; set; }

        public bool IsInNewMode { get; set; }

        public Prism.Events.IEventAggregator EventAggregator { get; set; }

        public ViewModels.IEntityListViewModelBase ActiveListNM { get; set; }
    }
}
