using Core.Features.Data.Enums;
using Core.Features.Extensions;
using DayLight.Core;
using DayLight.Core.API;
using DayLight.Core.API.Database;
using DayLight.Core.API.Events.EventArgs;
using DayLight.Core.API.Events.Handlers;
using DayLight.Core.API.Features;
using DayLight.Core.Models;
using DayLight.Dependencys.Models;
using Exiled.API.Features;
using JetBrains.Annotations;
using PlayerRoles;
using System.Collections.Generic;

namespace DayLight.GameStore;

public class MoneyManager
{



    public static void BuyItem(Player player, GameStoreItemPrice gameStoreItem)
    {
        if (player.DoNotTrack) return;
        var dbplayer = player.GetAdvancedPlayer().DatabasePlayer;
        if (dbplayer == null) return;
        dbplayer.Stats.Money -= gameStoreItem.Price;
        if (gameStoreItem.IsAmmo)
            foreach (var items in gameStoreItem.AmmoTypes)
            {
                player.AddAmmo(items.Key, items.Value);
            }
        else
            foreach (var items in gameStoreItem.ItemTypes)
            {
                player.AddItem(items);
            }
        GameStoreHandler.OnBuyingItem(player, gameStoreItem, gameStoreItem.Price);
        player.SendHint(ScreenZone.Notifications, GameStorePlugin.Instance.Translation.BoughtItemBHint.Replace("(item)", gameStoreItem.Name), 3);
    }
    public static bool CanRemoveMoneyFromPlayer(Player player, float reward)
    {
        if (player == null) return false;
        if (player.DoNotTrack) return false;
        var dbplayer = player.GetAdvancedPlayer().DatabasePlayer;
        return dbplayer.Stats.Money >= reward;
    }
    public static void AddMoneyToSteam64ID(ulong steamid, int money)
    {
        var players = DayLightDatabase.Database.GetCollection<DatabasePlayer>("players");
        var dbplayer = players.FindOne(x => x.SteamID == steamid);
        if (dbplayer == null) return;
        dbplayer.Stats.Money += money;
        if (dbplayer.Stats.Money < 0) dbplayer.Stats.Money = 0;
        players.Update(dbplayer);
    }
    public static void AddMoneyToPlayer(Player player, int money)
    {
        if (player == null) return;
        if (player.DoNotTrack || money == 0) return;
        var dbplayer = player.GetAdvancedPlayer().DatabasePlayer;
        if (dbplayer == null) return;
        if (dbplayer.Stats.Money < 0) dbplayer.Stats.Money = 0;
        var moneyEventArgs = new GainedMoneyEventArgs(player,new GameStoreReward
        {
            Name = "AddMoney", Money = new Dictionary<RoleTypeId, int>
            {
                { RoleTypeId.None, money }
            },
            MaxPerRound = -1
        }, money);
        GameStoreHandler.OnGainingMoney(moneyEventArgs);
        
        dbplayer.Stats.Money += money;
    }
    public static void AddRewardToPlayer(Player player, GameStoreReward gameStoreReward)
    {
        var dbplayer = player?.GetAdvancedPlayer().DatabasePlayer;

        if (dbplayer == null) return;

        Logger.Info("here");
        if (gameStoreReward.Money.ContainsKey(player.Role.Type))
        {
            var advancedPlayer = AdvancedPlayer.Get(player);
            if (advancedPlayer != null && advancedPlayer.GameStoreRewardLimit.ContainsKey(gameStoreReward.Name))
            {
                var amount = advancedPlayer.GameStoreRewardLimit[gameStoreReward.Name];
                if (amount >= gameStoreReward.MaxPerRound && gameStoreReward.MaxPerRound != -1)
                {
                    return;
                }

                advancedPlayer.GameStoreRewardLimit[gameStoreReward.Name]++;
            }
            else
            {
                if (advancedPlayer != null) advancedPlayer.GameStoreRewardLimit.Add(gameStoreReward.Name, 1);
            }
            if (gameStoreReward.Money[player.Role.Type] == 0) return;
            if (gameStoreReward.Money[player.Role.Type] == 0) return;
            var moneyEventArgs = new GainedMoneyEventArgs(player, gameStoreReward, gameStoreReward.Money[player.Role.Type] * DayLightCore.Instance.Config.MoneyMuliplier);
            GameStoreHandler.OnGainingMoney(moneyEventArgs);

            
            dbplayer.Stats.Money += gameStoreReward.Money[player.Role.Type];

        }
        else if (gameStoreReward.Money.ContainsKey(RoleTypeId.None))
        {
            Logger.Info("here2");

            var adv = AdvancedPlayer.Get(player);
            if (adv != null && adv.GameStoreRewardLimit.ContainsKey(gameStoreReward.Name))
            {
                var amount = adv.GameStoreRewardLimit[gameStoreReward.Name];
                if (amount >= gameStoreReward.MaxPerRound && gameStoreReward.MaxPerRound != -1)
                {
                    return;
                }

                adv.GameStoreRewardLimit[gameStoreReward.Name]++;
            }
            else
            {
                if (adv != null) adv.GameStoreRewardLimit.Add(gameStoreReward.Name, 1);
            }

            if (gameStoreReward.Money[RoleTypeId.None] == 0) return;

            var moneyEventArgs = new GainedMoneyEventArgs(player, gameStoreReward, gameStoreReward.Money[RoleTypeId.None] * DayLightCore.Instance.Config.MoneyMuliplier);

            GameStoreHandler.OnGainingMoney(moneyEventArgs);
            dbplayer.Stats.Money += gameStoreReward.Money[RoleTypeId.None];

        }
        else
        {
            return;
        }

        if (dbplayer.Stats.Money < 0) dbplayer.Stats.Money = 0;
        if (dbplayer.Stats.Money > DayLightCore.Instance.Config.MoneyLimit && DayLightCore.Instance.Config.EnableLimit)
            dbplayer.Stats.Money = DayLightCore.Instance.Config.MoneyLimit;
    }
    [UsedImplicitly]
    public static float GetMoneyFromSteam64ID(ulong steam64id)
    {
        var players = DayLightDatabase.Database.GetCollection<DatabasePlayer>("players");

        var dbplayer = players.FindOne(x => x.SteamID == steam64id);

        if (dbplayer != null)
            return dbplayer.Stats.Money;
        return 0;
    }
    
}
