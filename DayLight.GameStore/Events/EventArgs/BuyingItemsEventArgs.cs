using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using PluginAPI.Core;
using Player = Exiled.API.Features.Player;

namespace DayLight.GameStore.Events.EventArgs;

public class BuyingItemsEventArgs : IExiledEvent
{
    public Player Player { get; set; }
    public Configs.ItemPrice Item { get; set; }

    public int Price { get; }

    public BuyingItemsEventArgs(Player player,Configs.ItemPrice itemPrice, int price)
    {
        Item = itemPrice;
        Player = player;
        Price = price;
    }
}
