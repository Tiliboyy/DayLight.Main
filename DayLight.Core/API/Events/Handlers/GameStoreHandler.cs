﻿using DayLight.Core.API.Events.EventArgs;
using Exiled.API.Features;

namespace DayLight.Core.API.Events.Handlers;

public class GameStoreHandler
{
    public static Exiled.Events.Features.Event<GainedMoneyEventArgs> GainingMoney = new();
    public static Exiled.Events.Features.Event<BuyingItemsEventArgs> BuyingItems = new();
    

    public static void OnGainingMoney(Player player, Reward reward, int amount)
    {
        GainingMoney?.InvokeSafely(new GainedMoneyEventArgs(player,reward, amount));
    }
    public static void OnBuyingItem(Player player, ItemPrice reward, int price)
    {
        BuyingItems?.InvokeSafely(new BuyingItemsEventArgs(player, reward, price));
    }

}