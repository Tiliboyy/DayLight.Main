using DayLight.Core.API.Events.EventArgs;
using Neuron.Core.Events;

namespace DayLight.Core.API.Events.Handlers;

public class RemoteAdmin
{
    public static EventReactor<RequestingPlayerDataEventArgs> RequestingPlayerData = new();

    
}
