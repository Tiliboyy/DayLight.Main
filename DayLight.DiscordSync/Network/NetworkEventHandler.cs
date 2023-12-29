using DayLight.Core;
using DayLight.Core.API;
using DayLight.Core.API.Database;
using DayLight.Dependencys.Communication;
using DayLight.Dependencys.Communication.Enums;
using DayLight.Dependencys.Stats;
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
        Log.Debug(ev.Type);
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
                case MessageType.Link:
                    HandleLink(ev);
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
                case MessageType.List:
                case MessageType.String:
                case MessageType.RoleUpdate:
                case MessageType.None:
                    break;
                case MessageType.Webhook:
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
        if(!int.TryParse(ev.GetData<StringMessage>().String, out int Money))
            return;
        var dbPlayer = DayLightDatabase.GetDatabasePlayer(Link.LinkDatabase.GetLinkedSteamID(ev.UserID));
        if (dbPlayer != null && dbPlayer.Stats != null)
            dbPlayer.Stats.Money += Money;

    }
    private static void HandleLeaderboard(ReceivedFullEventArgs ev)
    {
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.Leaderboard, new StringMessage(Leaderboard.GetLeaderboard(Leaderboard.GetLeaderboardType(ev.GetData<StringMessage>().String))), "");
    }

    private static void HandlePing(ReceivedFullEventArgs ev)
    {
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.Ping, "pong", "");
    }
    private static void HandleLinkCheck(ReceivedFullEventArgs ev)
    {
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.CheckLink, new LinkMessage(){Linked = Link.LinkDatabase.IsUserIdLinked(ev.UserID)}, GetNickname(ev.UserID));    }
    private static void HandleLink(ReceivedFullEventArgs ev)
    {
        var linker = ev.GetData<LinkMessage>();
        try
        {
            var flag = Link.LinkDatabase.Link(linker.UserId, linker.Code);
            linker.Linked = flag;
        }
        catch (Exception exception)
        {
            Log.Error(exception);
            throw;
        }
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.Link, linker, GetNickname(ev.UserID));
    }
    private static void HandleDatabasePlayer(ReceivedFullEventArgs ev)
    {
        try
        {

            var ply = Database.LiteDatabase.GetCollection<Player>().FindOne(x => x.Id == Link.LinkDatabase.GetLinkedSteamID(ev.UserID).ToString());
            var playtime = new TimeSpan(0, 0, ply.PlayTimeRecords.Sum(pt => pt.Value)).TotalSeconds;

            var DatabaseEntry = DayLightDatabase.GetDatabasePlayer(Link.LinkDatabase.GetLinkedSteamID(ev.UserID));
            var databasePlayerSender = new PlayerInformation(DatabaseEntry, playtime);
            _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.PlayerInformation, databasePlayerSender, GetNickname(ev.UserID));

        }
        catch (Exception exception)
        {
            Log.Error(exception);
            throw;
        }
    }
    private static void HandlePlayerSettings(ReceivedFullEventArgs ev)
    {
        var IsPublic = ev.GetData<bool>();
        var DatabaseEntry = DayLightDatabase.GetDatabasePlayer(Link.LinkDatabase.GetLinkedSteamID(ev.UserID));

        if (DatabaseEntry != null) DatabaseEntry.Stats.Public = IsPublic;
    }
    private static void HandleGameStoreMoney(ReceivedFullEventArgs ev)
    {
        var dbPlayer = DayLightDatabase.GetDatabasePlayer(Link.LinkDatabase.GetLinkedSteamID(ev.UserID));
        float money = 0;
        if (dbPlayer != null && dbPlayer.Stats != null)
            money = dbPlayer.Stats.Money;
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.String, money, "");
    }
    private static void HandlePlayerList(ReceivedFullEventArgs ev)
    {
        var names = Exiled.API.Features.Player.List.Select(x => x.Nickname).ToList();
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.PlayerList, names, "");
    }

    public static string GetNickname(ulong UserID)
    {

        var e = DayLightDatabase.GetNicknameFromSteam64ID(Link.LinkDatabase.GetLinkedSteamID(UserID));
        return e ?? "None";
    }
}