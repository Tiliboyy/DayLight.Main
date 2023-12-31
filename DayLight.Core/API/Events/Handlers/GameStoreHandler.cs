﻿using DayLight.Core.API.Events.EventArgs;
using DayLight.Core.Models;
using Exiled.API.Features;

namespace DayLight.Core.API.Events.Handlers;

public class GameStoreHandler
{
    public static Exiled.Events.Features.Event<GainedMoneyEventArgs> GainingMoney = new();
    public static Exiled.Events.Features.Event<BuyingItemsEventArgs> BuyingItems = new();
    

    public static void OnGainingMoney(GainedMoneyEventArgs ev)
    {
        GainingMoney?.InvokeSafely(ev);
    }
    public static void OnBuyingItem(Player player, GameStoreItemPrice reward, int price)
    {
        BuyingItems?.InvokeSafely(new BuyingItemsEventArgs(player, reward, price));
    }

}
