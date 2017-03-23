
using Prism.Events;

namespace POS.DesktopClient.Events.ManagmentEvents
{

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
