using DayLight.Core.API.Events.EventArgs;
using DayLight.Core.API.Events.Handlers;
using DayLight.Core.API.Features;
using DayLight.Core.Models;
using Exiled.API.Features;
using PlayerRoles;
using System.Collections.Generic;

namespace DayLight.Core.API;

public static class Extensions
{
    public static AdvancedPlayer GetAdvancedPlayer(this Player player) => AdvancedPlayer.Get(player);
    public static float GetMoney(this Player player)
    {
        return player.GetAdvancedPlayer().DatabasePlayer.Stats.Money;
    }
    public static void GiveMoney(this Player player, int money)
    {
        Logger.Debug($"Giving {money} Money to {player.Nickname}");
        var databasePlayer = player.GetAdvancedPlayer().DatabasePlayer;
        if (databasePlayer == null) return;
        if (databasePlayer.Stats.Money < 0) databasePlayer.Stats.Money = 0;
        var moneyEventArgs = new GainedMoneyEventArgs(player,new GameStoreReward
        {
            Name = "AddMoney", Money = new Dictionary<RoleTypeId, int>
            {
                { RoleTypeId.None, money }
            },
            MaxPerRound = -1
        }, money);
        GameStoreHandler.OnGainingMoney(moneyEventArgs);
            
        databasePlayer.Stats.Money += money;
        
    }

    public static ulong GetSteam64Id(this Player player) => ulong.Parse(player.RawUserId);
}
