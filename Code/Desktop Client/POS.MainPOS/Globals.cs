using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.MainPOS
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

        public Prism.Events.IEventAggregator EventAggregator;
    }
}
