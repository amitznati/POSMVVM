using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataGridFilterLibrary
{
    public class BugFix
    {
        private BugFix()
        { }
        private static BugFix _instance = new BugFix();
        public static BugFix Instance()
        {
             return _instance; 
        }

        public bool IsToRunQuery { get; set; }
    }
}
