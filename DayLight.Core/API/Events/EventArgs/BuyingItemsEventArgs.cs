using DayLight.Core.Models;
using Neuron.Core.Events;
using Player = Exiled.API.Features.Player;

namespace DayLight.Core.API.Events.EventArgs;

public class BuyingItemsEventArgs : IEvent
{
    public Player Player { get; set; }
    public GameStoreItemPrice GameStoreItem { get; set; }

    public int Price { get; }

    public BuyingItemsEventArgs(Player player,GameStoreItemPrice gameStoreItemPrice, int price)
    {
        GameStoreItem = gameStoreItemPrice;
        Player = player;
        Price = price;
    }
}
