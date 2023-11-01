
using DayLight.Core.API.Database;
using Exiled.API.Features;

namespace DayLight.Core.API;

public static class Extensions
{
    public static float GetMoney(this Player player)
    {
        return DayLightDatabase.GetPlayerMoney(player);
    }
    
    public static void GiveReward(this Player player, Reward reward)
    {
        DayLightDatabase.AddRewardToPlayer(player, reward);
    }
    
    public static void GiveMoney(this Player player, int money)
    {
        DayLightDatabase.AddMoneyToPlayer(player, money);
    }
}
