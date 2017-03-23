using POSEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSEntities
{
    public interface IDisplayableModel 
    {
        ICollection<ItemDisplayInfo> ItemDisplayInfoes { get; set; }
        ItemDisplayInfo GetNewDiplayInfo();
    }
}
