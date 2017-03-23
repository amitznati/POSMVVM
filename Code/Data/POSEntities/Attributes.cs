using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSEntities
{
    public class MyDisplayAttribute : Attribute
    {
        public MyDisplayAttribute() { }
        public MyDisplayAttribute(bool isVisible, string name)
        {
            Name = name;
            IsVisible = isVisible;
        }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
    }
}
