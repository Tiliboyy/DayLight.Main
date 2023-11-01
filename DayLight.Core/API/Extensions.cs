
using DayLight.Core.API.Database;
using DayLight.DiscordSync.Dependencys;
using DayLight.DiscordSync.Dependencys.Stats;
using Exiled.API.Features;
using JetBrains.Annotations;

namespace DayLight.Core.API;

public static class Extensions
{
    public static float GetMoney(this Player player)
    {
        return DayLightDatabase.GameStore.GetPlayerMoney(player);
    }
    public static void GiveMoney(this Player player, int money)
    {
        DayLightDatabase.GameStore.AddMoneyToPlayer(player, money);
    }
    public static void GiveGameStoreReward(this Player player, Reward reward)
    {
        DayLightDatabase.GameStore.AddRewardToPlayer(player, reward);
    }
    [CanBeNull]
    public static DatabasePlayer GetDBPlayer(this Player player) => DayLightDatabase.GetDBPlayer(player);
    
    public static void AddStatsDataToPlayer(this Player player, StatTypes types, double number, ItemType itemTypeid = ItemType.None) => DayLightDatabase.Stats.AddStatsDataToPlayer(player, types, number, itemTypeid);

}
