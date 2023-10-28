using Core.Features.Data.Enums;
using Core.Features.Extensions;
using DayLight.GameStore.Components;
using DayLight.GameStore.Configs;
using Exiled.API.Features;
using JetBrains.Annotations;
using LiteDB;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Paths = Exiled.API.Features.Paths;
using Player = Exiled.API.Features.Player;

namespace DayLight.GameStore;

public static class GameStoreDatabase
{


    public static LiteDatabase db = new(Path.Combine(Paths.Configs, "Gamestore/GameStore.db"));
    public struct Pay
    {
        public string TargetId { get; set; }
        public float Amount { get; set; }
        public DateTime Date { get; set; }
    }
    public class DatabasePlayer
    {
        public string _id { get; set; }
        public float Money { get; set; }

        public string Nickname { get; set; }


    }

    public static class Database
    {
        public static void CreatePlayers()
        {
            var players = db.GetCollection<DatabasePlayer>("players");

            if (!players.Exists(x => true)) players.EnsureIndex(x => x._id);
        }

        public static void AddPlayer(Player player)
        {
            if (player == null) return;
            if (player.DoNotTrack) return;
            var playerID = player.RawUserId.Split('@')[0];
            var players = db.GetCollection<DatabasePlayer>("players");


            if (players.FindOne(x => x._id == playerID) != null)
            {
                var udbplayer = players.FindOne(x => x._id == playerID);
                udbplayer.Nickname = player.Nickname;
                players.Update(udbplayer);
                return;
            }

            players.Insert(new DatabasePlayer
            {
                _id = playerID,
                Money = 0,
                Nickname = player.Nickname
            });
            var dbplayer = players.FindOne(x => x._id == playerID);
            players.Update(dbplayer);
        }


        public static void BuyItem(Player player, ItemPrice item)
        {
            if (player.DoNotTrack) return;
            var playerID = player.RawUserId.Split('@')[0];
            var players = db.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x._id == playerID);

            if (dbplayer == null) return;
            dbplayer.Money -= item.Price;
            if (item.IsAmmo)
                foreach (var items in item.AmmoTypes)
                    player.AddAmmo(items.Key, items.Value);
            else
                foreach (var items in item.ItemTypes)
                    player.AddItem(items);
            Events.Handlers.OnBuyingItem(player, item, item.Price);
            player.SendHint(ScreenZone.Notifications, GameStorePlugin.Instance.Translation.BoughtItemBHint.Replace("(item)", item.Name), 3);
            players.Update(dbplayer);
        }

        public static bool CanRemoveMoneyFromPlayer(Player player, float reward)
        {
            if (player == null) return false;
            if (player.DoNotTrack) return false;
            var playerID = player.RawUserId.Split('@')[0];
            var players = db.GetCollection<DatabasePlayer>("players");
            var dbplayer = players.FindOne(x => x._id == playerID);

            if (dbplayer != null) return dbplayer.Money >= reward;

            return false;
        }
        public static void AddMoneyToSteam64ID(ulong steamid, int money)
        {
            var players = db.GetCollection<DatabasePlayer>("players");
            var dbplayer = players.FindOne(x => x._id != null && x._id == steamid.ToString());
            if (dbplayer == null) return;
            dbplayer.Money += money;
            if (dbplayer.Money < 0) dbplayer.Money = 0;
            players.Update(dbplayer);
        }

        public static void AddMoneyToPlayer(Player player, int money)
        {
            if (player == null) return;
            if (player.DoNotTrack || money == 0) return;
            var playerID = player.RawUserId.Split('@')[0];
            var players = db.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x._id != null && x._id == playerID);

            if (dbplayer == null) return;
            dbplayer.Money += money;
            if (dbplayer.Money < 0) dbplayer.Money = 0;
            Events.Handlers.OnGainingMoney(player,
                new Reward()
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
        public static void AddRewardToPlayer(Player player, Reward reward)
        {
            if (player == null) return;
            var playerID = player.RawUserId.Split('@')[0];
            var players = db.GetCollection<DatabasePlayer>("players");
            var dbplayer = players.FindOne(x => x._id != null && x._id == playerID);

            if (dbplayer == null) return;

            if (reward.Money.ContainsKey(player.Role.Type))
            {
                if (player.GameObject.GetComponent<GameStoreComponent>().RewardLimit.ContainsKey(reward.Name))
                {
                    var amount = player.GameObject.GetComponent<GameStoreComponent>().RewardLimit[reward.Name];
                    if (amount >= reward.MaxPerRound && reward.MaxPerRound != -1)
                    {
                        return;
                    }

                    player.GameObject.GetComponent<GameStoreComponent>().RewardLimit[reward.Name]++;
                }
                else
                {
                    player.GameObject.GetComponent<GameStoreComponent>().RewardLimit.Add(reward.Name, 1);
                }
                if (reward.Money[player.Role.Type] == 0) return;
                dbplayer.Money += reward.Money[player.Role.Type] * GameStorePlugin.MoneyMuliplier;
                Events.Handlers.OnGainingMoney(player, reward, reward.Money[player.Role.Type] * GameStorePlugin.MoneyMuliplier);


            }
            else if (reward.Money.ContainsKey(RoleTypeId.None))
            {

                if (player.GameObject.GetComponent<GameStoreComponent>().RewardLimit.ContainsKey(reward.Name))
                {
                    var amount = player.GameObject.GetComponent<GameStoreComponent>().RewardLimit[reward.Name];
                    if (amount >= reward.MaxPerRound && reward.MaxPerRound != -1)
                    {
                        return;
                    }

                    player.GameObject.GetComponent<GameStoreComponent>().RewardLimit[reward.Name]++;
                }
                else
                {
                    player.GameObject.GetComponent<GameStoreComponent>().RewardLimit.Add(reward.Name, 1);
                }
                dbplayer.Money += reward.Money[RoleTypeId.None] * GameStorePlugin.MoneyMuliplier;
                Events.Handlers.OnGainingMoney(player, reward, reward.Money[RoleTypeId.None] * GameStorePlugin.MoneyMuliplier);
                
            }
            else
            {
                return;
            }

            if (dbplayer.Money < 0) dbplayer.Money = 0;
            if (dbplayer.Money > GameStorePlugin.Instance.Config.MoneyLimit && GameStorePlugin.Instance.Config.EnableLimit)
                dbplayer.Money = GameStorePlugin.Instance.Config.MoneyLimit;

            players.Update(dbplayer);
        }
        public static string GetPlayerLeaderboard(Player player)
        {
            var list = GetLeaderboard();
            var playerID = player.RawUserId.Split('@')[0];

            var plypos = list.IndexOf(list.First(X => X._id == playerID));
            var leaderboard = list.Take(10).ToList();
            var i = 1;
            var hasply = false;
            var str = "\n";
            foreach (var databasePlayer in leaderboard)
            {
                if (databasePlayer.Nickname == null)
                {
                    str += $"\n[{i}] {databasePlayer._id}: {databasePlayer.Money} {GameStorePlugin.Instance.Translation.CurrencyName}";
                }
                else
                {

                    if (databasePlayer.Nickname == player.Nickname)
                        hasply = true;
                    str += $"\n[{i}] {databasePlayer.Nickname}: {databasePlayer.Money} {GameStorePlugin.Instance.Translation.CurrencyName}";
                }

                i++;
            }
            if(!hasply)
                str += $"\n\n[{plypos + 1}] {player.Nickname}";
            return str;
        }
        public static List<DatabasePlayer> GetLeaderboard()
        {
            var players = db.GetCollection<DatabasePlayer>("players");
            var e = players.FindAll().OrderByDescending(p => p.Money).ToList();
            return e;
        }

        public static float GetPlayerMoney(Player player)
        {
            if (player.DoNotTrack) return 0;
            var playerID = player.RawUserId.Split('@')[0];
            var players = db.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x._id == playerID);

            if (dbplayer != null)
                return dbplayer.Money;
            return 0;
        }
        [UsedImplicitly]
        public static float GetMoneyFromSteam64ID(string steam64id)
        {
            var playerID = steam64id.Split('@')[0];
            var players = db.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x._id == playerID);

            if (dbplayer != null)
                return dbplayer.Money;
            return 0;
        }
        [UsedImplicitly]
        public static string GetNicknameFromSteam64ID(string steam64id)
        {
            var playerID = steam64id.Split('@')[0];
            var players = db.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x._id == playerID);

            return dbplayer != null ? dbplayer.Nickname : "None";
        }

        public static void RemovePlayer(Player player)
        {
            if (player == null) return;
            var playerID = player.RawUserId.Split('@')[0];
            var players = db.GetCollection<DatabasePlayer>("players");
            var dbplayer = players.FindOne(x => x._id == playerID);

            if (dbplayer != null) players.Delete(dbplayer._id);
        }
    }
}