
using DayLight.Core.API.Database;
using DayLight.Core.API.Features;
using DayLight.Core.Models;
using DayLight.DiscordSync.Dependencys;
using DayLight.DiscordSync.Dependencys.Stats;
using Exiled.API.Features;
using JetBrains.Annotations;
using System;

namespace DayLight.Core.API;

public static class Extensions
{
    public static AdvancedPlayer GetAdvancedPlayer(this Player player) => AdvancedPlayer.Get(player);
    public static float GetMoney(this Player player)
    {
        return DayLightDatabase.GameStore.GetPlayerMoney(player);
    }
    public static void GiveMoney(this Player player, int money)
    {
        DayLightDatabase.GameStore.AddMoneyToPlayer(player, money);
    }
    public static void GiveGameStoreReward(this Player player, GameStoreReward gameStoreReward)
    {
        DayLightDatabase.GameStore.AddRewardToPlayer(player, gameStoreReward);
    }
    public static void AddStatsDataToPlayer(this Player player, StatTypes types, double number, ItemType itemTypeid = ItemType.None) => DayLightDatabase.Stats.AddStatsDataToPlayer(player, types, number, itemTypeid);

}
