﻿#region

using DayLight.Core;
using DayLight.Core.API.Database;
using DayLight.DiscordSync.Dependencys.Stats;
using Exiled.API.Features;
using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

#endregion

namespace DayLight.Moderation.Warn;

public static class WarnDatabase
{
    
    public static class Database
    {

        public static string RemoveWarn(string steam64id, int id)
        {
            var playerID = steam64id.Split('@')[0];
            var players = DayLightDatabase.Database.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x._id == playerID);
            if (dbplayer.Warns == null) return "Spieler wurde nicht gefunden";
            var foundwarn = false;
            foreach (var warn in dbplayer.Warns.Where(warn => warn.Id == id))
            {
                dbplayer.Warns.Remove(warn);
                foundwarn = true;
                players.Update(dbplayer);
                break;
            }

            return foundwarn ? "Verwarnung wurde gelöscht" : "Verwarnung wurde nicht gefunden";


        }

        public static string AddWarn(string warned, string warner, float points, string reason, DateTime? time)
        {
            try
            {
                var playerID = warned.Split('@')[0];
                var players = DayLightDatabase.Database.GetCollection<DatabasePlayer>("players");

                var dbplayer = players.FindOne(x => x._id == playerID);

                int max = 0;
                if (dbplayer.Warns != null && dbplayer.Warns.Count != 0)
                {
                    max = dbplayer.Warns.Select(keyValue => keyValue.Id).Prepend(max).Max();
                    max += 1;
                }

                var warn = new DiscordSync.Dependencys.Warn
                {
                    Reason = reason, Points = points, WarnerUsername = warner, Date = time ?? DateTime.Now.Date, Id = max
                };
                dbplayer.Warns ??= new List<DiscordSync.Dependencys.Warn>();
                dbplayer.Warns.Add(warn);
                players.Update(dbplayer);
                return "Spieler wurde verwarnt";
            }
            catch (Exception e)
            {
                Logger.Error(e);
                throw;
            }
        }

        public static string GetWarns(string steamid, bool onlynew, out bool haswarns)
        {
            var playerID = steamid.Split('@')[0];
            var players = DayLightDatabase.Database.GetCollection<DatabasePlayer>("players");
            var dbplayer = players.FindOne(x => x._id == playerID);
            if (dbplayer == null)
            {
                haswarns = false;
                return "Dieser Spieler hat keine Verwarnungen";
            }

            if (dbplayer.Warns == null || dbplayer.Warns.Count == 0)
            {
                haswarns = false;
                return "Dieser Spieler hat keine Verwarnungen";

            } 
            string builder = "\n------------------------------------------------\n";
            float total = 0;

            
            
            if (onlynew)
            {
                    var newWarns = (from warn in dbplayer.Warns
                        let date = warn.Date.AddDays(30)
                        let span = DateTime.Now - warn.Date
                        where span.Days <= 30
                        select warn).ToList();
                    if (newWarns.Count == 0)
                    {
                        haswarns = false;
                        return "Dieser Spieler hat keine Verwarnungen";
                    }
                    foreach (var warn in newWarns)
                    {
                        var span = DateTime.Now - warn.Date;
                        total += warn.Points;

                        builder +=
                            $"Verwarnung({warn.Id})" +
                            $"\nGrund: {warn.Reason}" +
                            $"\nPunkte: {warn.Points}" +
                            $"\nModerator: {warn.WarnerUsername}" +
                            $"\nVor {span.Days} Tagen" +
                            $"\n------------------------------------------------\n";
                    }

                    haswarns = true;
                    return builder + "\n\nTotal: " + GetTotal(steamid) + " Punkte";
            }
            
            var oldwarns = dbplayer.Warns;
            foreach (var warn in oldwarns)
            {
                var span = DateTime.Now - warn.Date;
                total += warn.Points;

                builder +=
                    $"Verwarnung({warn.Id})" +
                    $"\nGrund: {warn.Reason}" +
                    $"\nPunkte: {warn.Points}" +
                    $"\nModerator: {warn.WarnerUsername}" +
                    $"\nVor {span.Days} Tagen" +
                    $"\n------------------------------------------------\n";
            }
            haswarns = true;
            return builder + "\n\nTotal: " + GetTotal(steamid) + " Punkte";
        }

        public static float GetTotal(string steamid)
        {
            var playerID = steamid.Split('@')[0];
            var players = DayLightDatabase.Database.GetCollection<DatabasePlayer>("players");

            var dbplayer = players.FindOne(x => x._id == playerID);

            if (dbplayer?.Warns == null)
            {
                return 0;
            }

            if (dbplayer.Warns.Count == 0)
                return 0;
            var newWarns = (from warn in dbplayer.Warns
                let date = warn.Date.AddDays(30)
                let span = DateTime.Now - warn.Date
                where span.Days <= 30
                select warn).ToList();
            return (from warn in newWarns let span = DateTime.Now - warn.Date select warn.Points).Sum();
        }
        
    }

}