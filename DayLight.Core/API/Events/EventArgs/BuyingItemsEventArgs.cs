using Neuron.Core.Events;
using Player = Exiled.API.Features.Player;

namespace DayLight.Core.API.Events.EventArgs;

public class BuyingItemsEventArgs : IEvent
{
    public Player Player { get; set; }
    public ItemPrice Item { get; set; }

    public int Price { get; }

    public BuyingItemsEventArgs(Player player,ItemPrice itemPrice, int price)
    {
        Item = itemPrice;
        Player = player;
        Price = price;
    }
}
