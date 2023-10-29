using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using Neuron.Core.Events;
using PluginAPI.Core;
using Player = Exiled.API.Features.Player;

namespace DayLight.GameStore.Events.EventArgs;

public class GainedMoneyEventArgs : IEvent
{
    public Player Player { get; }
    
    public int Amount { get; }
    public Configs.Reward Reward { get; }

    public GainedMoneyEventArgs(Player player, Configs.Reward reward, int amount)
    {
        Reward = reward;
        Player = player;
        Amount = amount;
    }
}
