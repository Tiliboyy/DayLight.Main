using DayLight.Core.Models;
using Neuron.Core.Events;
using Player = Exiled.API.Features.Player;

namespace DayLight.Core.API.Events.EventArgs;

public class GainedMoneyEventArgs : IEvent
{
    public Player Player { get; }
    
    public int Money { get; }
    public GameStoreReward GameStoreReward { get; }

    public GainedMoneyEventArgs(Player player, GameStoreReward gameStoreReward, int money)
    {  
        GameStoreReward = gameStoreReward;
        Player = player;
        Money = money;
    }
}
