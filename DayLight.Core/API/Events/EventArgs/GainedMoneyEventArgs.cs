using Neuron.Core.Events;
using Player = Exiled.API.Features.Player;

namespace DayLight.Core.API.Events.EventArgs;

public class GainedMoneyEventArgs : IEvent
{
    public Player Player { get; }
    
    public int Amount { get; }
    public Reward Reward { get; }

    public GainedMoneyEventArgs(Player player, Reward reward, int amount)
    {
        Reward = reward;
        Player = player;
        Amount = amount;
    }
}
