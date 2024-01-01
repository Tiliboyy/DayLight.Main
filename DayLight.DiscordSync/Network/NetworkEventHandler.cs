using DayLight.Core;
using DayLight.Core.API;
using DayLight.Core.API.Database;
using DayLight.Dependency.Enums;
using DayLight.Dependency.Models;
using DayLight.Dependency.Models.Helpers;
using DiscordSync.Plugin.Network.EventArgs.Network;
using Exiled.API.Features;
using System.Globalization;
using Database = SCPUtils.Database;
using Player = SCPUtils.Player;

namespace DiscordSync.Plugin.Network;

public static class NetworkEventHandler
{

    public static void DataReceived(object sender, ReceivedFullEventArgs ev)
    {
        Logger.Debug(ev.Type);
        Logger.Debug(ev.SerilzedData);
        try
        {
            switch (ev.Type)
            {
                case MessageType.PlayerList:
                    HandlePlayerList(ev);
                    break;
                case MessageType.PlayerInformation:
                    HandleDatabasePlayer(ev);
                    break;
                case MessageType.UpdateLink:
                    HandleUpdateLink(ev);
                    break;
                case MessageType.CheckLink:
                    HandleLinkCheck(ev);
                    break;
                case MessageType.Heartbeat:
                    return;
                case MessageType.Ping:
                    HandlePing(ev);
                    return;
                case MessageType.Leaderboard:
                    HandleLeaderboard(ev);
                    return;
                case MessageType.GiveMoney:
                    HandleAddMoney(ev);
                    break;
                case MessageType.ChangeProfilePublicState:
                    HandleProfilePublicState(ev);
                    break;
                case MessageType.Webhook:
                case MessageType.List:
                case MessageType.String:
                case MessageType.RoleUpdate:
                case MessageType.None:
                default:
                    Log.Error("Invalid request type recieved: " + ev.Type);
                    break;
            }
        }
        catch (Exception e)
        {
            Log.Error(e);
            throw;
        }

    }
    private static void HandleAddMoney(ReceivedFullEventArgs ev)
    {
        if(!int.TryParse(ev.GetData<StringHelper>().String, out int Money))
            return;
        var dbPlayer = DayLightDatabase.GetDatabasePlayer(Link.LinkDatabase.GetLinkedSteamID(ev.UserID));
        if (dbPlayer is { Stats: not null })
            dbPlayer.Stats.Money += Money;
        DayLightDatabase.UpdatePlayer(dbPlayer);


    }
    private static void HandleLeaderboard(ReceivedEventArgs ev)
    {
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.Leaderboard, new StringHelper(Leaderboard.GetLeaderboard(Leaderboard.GetLeaderboardType(ev.GetData<StringHelper>().String))), "");
    }

    private static void HandlePing(ReceivedFullEventArgs ev)
    {
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.Ping, "pong", "");
    }
    private static void HandleLinkCheck(ReceivedEventArgs ev)
    {
        var nickname = DayLightDatabase.GetDatabasePlayerDiscord(ev.UserID)?.Nickname ?? "None";
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.CheckLink, new LinkHelper()
        {
            Linked = Link.LinkDatabase.IsUserIdLinked(ev.UserID)
        }, nickname);
        
    }
    private static void HandleUpdateLink(ReceivedEventArgs ev)
    {
        var linkHelper = ev.GetData<LinkHelper>();
        try
        {
            var flag = Link.LinkDatabase.Link(linkHelper.UserId, linkHelper.Code);
            linkHelper.Linked = flag;
        }
        catch (Exception exception)
        {
            Log.Error(exception);
            throw;
        }
        var nickname = DayLightDatabase.GetDatabasePlayerDiscord(ev.UserID)?.Nickname ?? "None";
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.UpdateLink, linkHelper, nickname);
    }
    private static void HandleDatabasePlayer(ReceivedEventArgs ev)
    {
        try
        {
            var playtime = GetScpUtilsPlaytimeSeconds(ev);

            var DatabaseEntry = DayLightDatabase.GetDatabasePlayer(Link.LinkDatabase.GetLinkedSteamID(ev.UserID)) ?? new DatabasePlayer();
            var databasePlayerSender = new PlayerInformationHelper(DatabaseEntry, playtime);

            _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.PlayerInformation, databasePlayerSender, DatabaseEntry.Nickname);

        }
        catch (Exception exception)
        {
            Log.Error(exception);
            throw;
        }
    }
    private static void HandleProfilePublicState(ReceivedEventArgs ev)
    {
        var IsPublic = ev.GetData<BoolHelper>()!.Bool;
        var DatabaseEntry = DayLightDatabase.GetDatabasePlayer(Link.LinkDatabase.GetLinkedSteamID(ev.UserID));
        if (DatabaseEntry != null) DatabaseEntry.Stats.Public = IsPublic;
        DayLightDatabase.UpdatePlayer(DatabaseEntry);

    }
    private static void HandlePlayerList(ReceivedFullEventArgs ev)
    {
        var names = Exiled.API.Features.Player.List.Select(x => x.Nickname).ToList();
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.PlayerList, names, "");
    }
    
    private static double GetScpUtilsPlaytimeSeconds(ReceivedEventArgs ev)
    {
        var ply = Database.LiteDatabase.GetCollection<Player>().FindOne(x => x.Id == Link.LinkDatabase.GetLinkedSteamID(ev.UserID).ToString());
        var playtime = new TimeSpan(0, 0, ply.PlayTimeRecords.Sum(pt => pt.Value)).TotalSeconds;
        return playtime;
    }

}