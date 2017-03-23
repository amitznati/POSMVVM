using MenuDesign.Models;
using POSEntities.Entities;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_WPF.Events
{
    class AddItemEvent : PubSubEvent<MenuSetView>
    {
    }

    public class SelectedEntityChanged : PubSubEvent<object>
    {
    }

    public class SaveDialog : PubSubEvent<object> 

    {
    }

    public class SaveDialogComplete : PubSubEvent<object> { }

    public class CancelDialog : PubSubEvent<object>
    {
    }

    public class SaveDenied : PubSubEvent<object> { }

}
