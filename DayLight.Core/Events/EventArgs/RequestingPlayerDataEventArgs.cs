using Exiled.API.Features;
using Neuron.Core.Events;

namespace DayLight.Core.Events.EventArgs;

public class RequestingPlayerDataEventArgs : IEvent
{
    public Player Player { get; }
    public Player Target { get; set; }
    
    public string Message { get; set; }
    public bool IsAllowed { get; set; }
    
    public RequestingPlayerDataEventArgs(Player player, Player target,string message, bool isAllowed = true)
    {
        this.Player = player;
        this.Target = target;
        this.IsAllowed = isAllowed;
        this.Message = message;
    }
}
