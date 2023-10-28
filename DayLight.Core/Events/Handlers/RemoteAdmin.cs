using DayLight.Core.Events.EventArgs;
using Neuron.Core.Events;

namespace DayLight.Core.Events.Handlers;

public class RemoteAdmin
{
    public static EventReactor<RequestingPlayerDataEventArgs> RequestingPlayerData = new();
    public static void OnRequestingData(RequestingPlayerDataEventArgs ev) => RequestingPlayerData.Raise(ev);
    
}
