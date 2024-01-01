using DayLight.Dependency.Models;
using Exiled.API.Features;
using JetBrains.Annotations;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DayLight.Core.API.Database;

public static class DayLightDatabase
{
    public static LiteDatabase Database = new(Path.Combine(DayLightCore.Instance.Base.RelativePath("DayLight.Core"), "Database.db"));
    
    public static void UpdatePlayer(DatabasePlayer player)
    {
        var players = Database.GetCollection<DatabasePlayer>("players");
        players.Update(player);
    }
    [CanBeNull]
    public static DatabasePlayer GetDatabasePlayer(Player player)
    {
        try
        {
            if (player.DoNotTrack) return null;
        
            var playerID = player.GetSteam64Id();
            var players = Database.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x.SteamID == playerID);
            if(dbplayer == null) 
                Logger.Error($"Dbplayer was not found ({player.Nickname})");
            return dbplayer;
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }
    }
    [CanBeNull]
    public static DatabasePlayer GetDatabasePlayer(ulong Steam64ID)
    {
        try
        {
            var players = Database.GetCollection<DatabasePlayer>("players");
            var dbplayer = players.FindOne(x => x.SteamID == Steam64ID);
            if(dbplayer == null)
                Logger.Debug($"Database Entry of  {Steam64ID} was not found");
            return dbplayer;
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }
    }
    [CanBeNull]
    public static DatabasePlayer GetDatabasePlayerDiscord(ulong UserID)
    {
        try
        {
            var players = Database.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x.DiscordID == UserID);
            return dbplayer;
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }
    }

    public static void CreateDatabase()
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

        try
        {
            if (player.DoNotTrack) return;
            var playerID = player.GetSteam64Id();
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
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }
    }

    public static string GetNicknameFromSteam64ID(ulong steam64id)
    {
        var players = Database.GetCollection<DatabasePlayer>("players");
        var dbplayer = players.FindOne(x => x.SteamID == steam64id);
        return dbplayer != null ? dbplayer.Nickname : "None";
    }

    public static void RemovePlayer(Player player)
    {
        if (player == null) return;
        var playerID = ulong.Parse(player.RawUserId);
        var players = Database.GetCollection<DatabasePlayer>("players");
        var dbplayer = players.FindOne(x => x.SteamID == playerID);
        if (dbplayer != null) players.Delete(dbplayer.SteamID);
    }


    public class Stats
    {
        public static string GetPlayerLeaderboard(Player player)
        {
            var list = GetLeaderboard();
            var playerID = player.GetSteam64Id();

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
            if (!hasply)
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
