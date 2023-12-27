using DayLight.Core;
using DayLight.Core.API;
using DayLight.Core.API.Database;
using DayLight.Dependencys.Communication;
using DayLight.Dependencys.Communication.Enums;
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
                case MessageType.GameStoreMoney:
                    HandleGameStoreMoney(ev);
                    break;
                case MessageType.PlayerSettings:
                    HandlePlayerSettings(ev);
                    break;
                case MessageType.DatabasePlayer:
                    HandleDatabasePlayer(ev);
                    break;
                case MessageType.Playtime:
                    HandlePlaytime(ev);
                    break;
                case MessageType.Link:
                    HandleLink(ev);
                    break;
                case MessageType.LinkCheck:
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
                case MessageType.Achivements:
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
        var linkedSteamID = Link.LinkDatabase.GetLinkedSteamID(ev.UserID);
        if(!int.TryParse(ev.GetData<StringMessage>().String, out int i))
        {
            return;
        }
        //Todo: 
        DayLightDatabase.GameStore.AddMoneyToSteam64ID(linkedSteamID, i);
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
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.LinkCheck, new LinkMessage(){Linked = Link.LinkDatabase.IsUserIdLinked(ev.UserID)}, GetNickname(ev.UserID));    }
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
    private static void HandlePlaytime(ReceivedFullEventArgs ev)
    {
        Todo: 

        var player = Database.LiteDatabase.GetCollection<Player>().FindOne(x => x.Id == Link.LinkDatabase.GetLinkedSteamID(ev.UserID).ToString());
        var playtime = new TimeSpan(0, 0, player.PlayTimeRecords.Sum(pair => pair.Value)).TotalSeconds;
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.Playtime, playtime.ToString(CultureInfo.InvariantCulture), GetNickname(ev.UserID));
    }
    private static void HandleDatabasePlayer(ReceivedFullEventArgs ev)
    {
        try
        {

            var ply = Database.LiteDatabase.GetCollection<Player>().FindOne(x => x.Id == Link.LinkDatabase.GetLinkedSteamID(ev.UserID).ToString());
            var pt = new TimeSpan(0, 0, ply.PlayTimeRecords.Sum(pt => pt.Value)).TotalSeconds;

            var statssender = new DayLight.Dependencys.Stats.DatabasePlayerSender()
            {

                DatabasePlayer = DayLightDatabase.GetDatabasePlayerSteam64ID(Link.LinkDatabase.GetLinkedSteamID(ev.UserID)),
                Playtime = pt.ToString(CultureInfo.InvariantCulture),

            };
            _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.DatabasePlayer, statssender, GetNickname(ev.UserID));

        }
        catch (Exception exception)
        {
            Log.Error(exception);
            throw;
        }
    }
    private static void HandlePlayerSettings(ReceivedFullEventArgs ev)
    {
        var settings = ev.GetData<bool>();
        DayLightDatabase.GetDatabasePlayerSteam64ID(ev.UserID).Stats.Public = !ev.GetData<bool>();
    }
    private static void HandleGameStoreMoney(ReceivedFullEventArgs ev)
    {
        var moneyFromSteam64ID = DayLightDatabase.GameStore.GetMoneyFromSteam64ID(Link.LinkDatabase.GetLinkedSteamID(ev.UserID));
        _ = DiscordSyncPlugin.Instance.Network.ReplyLine(MessageType.String, moneyFromSteam64ID, "");
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