using Core.Features.Data.Enums;
using Core.Features.Extensions;
using DayLight.Core.API.Events.Handlers;
using DayLight.Core.API.Features;
using DayLight.Core.Models;
using DayLight.DiscordSync.Dependencys;
using DayLight.DiscordSync.Dependencys.Stats;
using JetBrains.Annotations;
using LiteDB;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Player = Exiled.API.Features.Player;

namespace DayLight.Core.API.Database;
public static class DayLightDatabase
{
    public static LiteDatabase Database = new(Path.Combine(DayLightCore.Instance.Base.RelativePath("DayLight.Core"),"Database.db"));

    public static void ChangeSettings(ulong steam64id,bool Private)
    {
        var players = Database.GetCollection<DatabasePlayer>("players");

        var dbplayer = players.FindOne(x => x.SteamID == steam64id.ToString());

        if(dbplayer.Private == Private)
            return;
        dbplayer.Private = Private;
        players.Update(dbplayer);

    }

    [CanBeNull]
    public static DatabasePlayer GetDBPlayer(Player player)
    {
        if (player.DoNotTrack) return null;
        var playerID = player.RawUserId.Split('@')[0];
        var players = Database.GetCollection<DatabasePlayer>("players");

        var dbplayer = players.FindOne(x => x.SteamID == playerID);

        return dbplayer;
    }

    public static void CreateCollection()
    {
        try
        {
            if (!Directory.Exists(DayLightCore.Instance.Base.RelativePath("DayLight.Core")))
            {
                Directory.CreateDirectory(DayLightCore.Instance.Base.RelativePath("DayLight.Core"));
            }
            var players = Database.GetCollection<DatabasePlayer>("players");

            if (!players.Exists(x => true)) players.EnsureIndex(x => x.SteamID);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }
    }

    public static void AddPlayer(Player player)
    {
        if (player == null) return;
        if (player.DoNotTrack) return;
        var playerID = player.RawUserId.Split('@')[0];
        var players = Database.GetCollection<DatabasePlayer>("players");


        if (players.FindOne(x => x.SteamID == playerID) != null)
        {
            var udbplayer = players.FindOne(x => x.SteamID == playerID);
            udbplayer.Nickname = player.Nickname;
            players.Update(udbplayer);
            return;
        }

        players.Insert(new DatabasePlayer(playerID, player.Nickname));
        var dbplayer = players.FindOne(x => x.SteamID == playerID);
        players.Update(dbplayer);
    }
    
    public static string GetNicknameFromSteam64ID(string steam64id)
    {
        var playerID = steam64id.Split('@')[0];
        var players = Database.GetCollection<DatabasePlayer>("players");

        var dbplayer = players.FindOne(x => x.SteamID == playerID);

        return dbplayer != null ? dbplayer.Nickname : "None";
    }

    public static void RemovePlayer(Player player)
    {
        if (player == null) return;
        var playerID = player.RawUserId.Split('@')[0];
        var players = Database.GetCollection<DatabasePlayer>("players");
        var dbplayer = players.FindOne(x => x.SteamID == playerID);

        if (dbplayer != null) players.Delete(dbplayer.SteamID);
    }


    public class GameStore
    {

        public static void BuyItem(Player player, GameStoreItemPrice gameStoreItem)
        {
            if (player.DoNotTrack) return;
            var playerID = player.RawUserId.Split('@')[0];
            var players = Database.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x.SteamID == playerID);

            if (dbplayer == null) return;
            dbplayer.Stats.Money -= gameStoreItem.Price;
            if (gameStoreItem.IsAmmo)
                foreach (var items in gameStoreItem.AmmoTypes)
                    player.AddAmmo(items.Key, items.Value);
            else
                foreach (var items in gameStoreItem.ItemTypes)
                    player.AddItem(items);
            GameStoreHandler.OnBuyingItem(player, gameStoreItem, gameStoreItem.Price);
            player.SendHint(ScreenZone.Notifications, DayLightCore.Instance.Config.BoughtItemHint.Replace("(item)", gameStoreItem.Name), 3);
            players.Update(dbplayer);
        }
        public static bool CanRemoveMoneyFromPlayer(Player player, float reward)
        {
            if (player == null) return false;
            if (player.DoNotTrack) return false;
            var playerID = player.RawUserId.Split('@')[0];
            var players = Database.GetCollection<DatabasePlayer>("players");
            var dbplayer = players.FindOne(x => x.SteamID == playerID);

            if (dbplayer != null) return dbplayer.Stats.Money >= reward;

            return false;
        }
        public static void AddMoneyToSteam64ID(ulong steamid, int money)
        {
            var players = Database.GetCollection<DatabasePlayer>("players");
            var dbplayer = players.FindOne(x => x.SteamID != null && x.SteamID == steamid.ToString());
            if (dbplayer == null) return;
            dbplayer.Stats.Money += money;
            if (dbplayer.Stats.Money < 0) dbplayer.Stats.Money = 0;
            players.Update(dbplayer);
        }
        public static void AddMoneyToPlayer(Player player, int money)
        {
            if (player == null) return;
            if (player.DoNotTrack || money == 0) return;
            var playerID = player.RawUserId.Split('@')[0];
            var players = Database.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x.SteamID != null && x.SteamID == playerID);

            if (dbplayer == null) return;
            dbplayer.Stats.Money += money;
            if (dbplayer.Stats.Money < 0) dbplayer.Stats.Money = 0;
            GameStoreHandler.OnGainingMoney(player,
                new GameStoreReward()
                {
                    Name = "AddMoney", Money = new Dictionary<RoleTypeId, int>()
                    {
                        { RoleTypeId.None, money },
                    },
                    MaxPerRound = -1
                },
                money);

            players.Update(dbplayer);
        }
        public static void AddRewardToPlayer(Player player, GameStoreReward gameStoreReward)
        {
            if (player == null) return;
            var playerID = player.RawUserId.Split('@')[0];
            var players = Database.GetCollection<DatabasePlayer>("players");
            var dbplayer = players.FindOne(x => x.SteamID != null && x.SteamID == playerID);

            if (dbplayer == null) return;

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
                dbplayer.Stats.Money += gameStoreReward.Money[player.Role.Type] * DayLightCore.Instance.Config.MoneyMuliplier;
                GameStoreHandler.OnGainingMoney(player, gameStoreReward, gameStoreReward.Money[player.Role.Type] * DayLightCore.Instance.Config.MoneyMuliplier);


            }
            else if (gameStoreReward.Money.ContainsKey(RoleTypeId.None))
            {

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
                dbplayer.Stats.Money += gameStoreReward.Money[RoleTypeId.None] * DayLightCore.Instance.Config.MoneyMuliplier;
                GameStoreHandler.OnGainingMoney(player, gameStoreReward, gameStoreReward.Money[RoleTypeId.None] * DayLightCore.Instance.Config.MoneyMuliplier);
            
            }
            else
            {
                return;
            }

            if (dbplayer.Stats.Money < 0) dbplayer.Stats.Money = 0;
            if (dbplayer.Stats.Money > DayLightCore.Instance.Config.MoneyLimit && DayLightCore.Instance.Config.EnableLimit)
                dbplayer.Stats.Money = DayLightCore.Instance.Config.MoneyLimit;

            players.Update(dbplayer);
        }
        public static float GetPlayerMoney(Player player)
        {
            if (player.DoNotTrack) return 0;
            var playerID = player.RawUserId.Split('@')[0];
            var players = Database.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x.SteamID == playerID);

            if (dbplayer != null)
                return dbplayer.Stats.Money;
            return 0;
        }
        [UsedImplicitly]
        public static float GetMoneyFromSteam64ID(string steam64id)
        {
            var playerID = steam64id.Split('@')[0];
            var players = Database.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x.SteamID == playerID);

            if (dbplayer != null)
                return dbplayer.Stats.Money;
            return 0;
        }
    }
    
    public class Stats
    {

        public static void AddStatsDataToPlayer(Player player, StatTypes types, double number, ItemType itemTypeid = ItemType.None)
        {
            if (!Enum.TryParse(itemTypeid.ToString(), out DiscordSync.Dependencys.Utils.ItemType itemType))
                return;

            if (player == null) return;
            if (player.DoNotTrack) return;
            var playerID = player.RawUserId.Split('@')[0];
            var players = Database.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x.SteamID != null && x.SteamID == playerID);

            if (dbplayer == null) return;
            switch (types)
            {
                case StatTypes.Kills:
                    dbplayer.Stats.Kills += number;
                    break;
                case StatTypes.Death:
                    dbplayer.Stats.Deaths += number;
                    break;
                case StatTypes.KD:
                    break;
                case StatTypes.Rounds:
                    dbplayer.Stats.PlayedRounds += number;
                    break;
                case StatTypes.EscapeTime:
                    if (number < dbplayer.Stats.FastestEscapeSeconds)
                        dbplayer.Stats.FastestEscapeSeconds = number;
                    break;
                case StatTypes.Items:
                    if (dbplayer.Stats.UsedItems.ContainsKey(itemType))
                    {
                        dbplayer.Stats.UsedItems[itemType]++;
                    }
                    else
                    {
                        dbplayer.Stats.UsedItems ??= new Dictionary<DiscordSync.Dependencys.Utils.ItemType, float>();
                        dbplayer.Stats.UsedItems.Add(itemType, 1);
                    }

                    break;
                case StatTypes.SCPKill:
                    dbplayer.Stats.KilledScps += number;
                    break;
                case StatTypes.PinkCandyKill:
                    dbplayer.Stats.PinkCandyKills += number;
                    break;
                case StatTypes.Achivement:
                    dbplayer.Stats.UnlockedAchivements.Add(number);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(types), types, null);
            }
            players.Update(dbplayer);
        }
        public static string GetPlayerLeaderboard(Player player)
        {
            var list = GetLeaderboard();
            var playerID = player.RawUserId.Split('@')[0];

            var plypos = list.IndexOf(list.First(X => X.SteamID == playerID));
            var leaderboard = list.Take(10).ToList();
            var i = 1;
            var hasply = false;
            var str = "\n";
            foreach (var databasePlayer in leaderboard)
            {
                if (databasePlayer.Nickname == null)
                {
                    str += $"\n[{i}] {databasePlayer.SteamID}: {databasePlayer.Stats.Money} {DayLightCore.Instance.Translation.CurrencyName}";
                }
                else
                {

                    if (databasePlayer.Nickname == player.Nickname)
                        hasply = true;
                    str += $"\n[{i}] {databasePlayer.Nickname}: {databasePlayer.Stats.Money} {DayLightCore.Instance.Translation.CurrencyName}";
                }

                i++;
            }
            if(!hasply)
                str += $"\n\n[{plypos + 1}] {player.Nickname}";
            return str;
        }
        public static List<DatabasePlayer> GetLeaderboard()
        {
            var players = Database.GetCollection<DatabasePlayer>("players");
            var e = players.FindAll().OrderByDescending(p => p.Stats.Money).ToList();
            return e;
        }
    }

    
}