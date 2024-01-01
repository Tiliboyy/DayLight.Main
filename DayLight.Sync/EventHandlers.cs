using DayLight.Core.API;
using DayLight.Dependency.Enums;
using DayLight.Dependency.Models.Communication;
using DayLight.Dependency.Models.Helpers;
using DayLight.Sync.Link;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using SCPUtils;
using Player = Exiled.API.Features.Player;
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace DayLight.Sync;

public class EventHandlers
{
    public static void OnReloadedRa()
    {
        foreach (var player in Player.List)
        {
            _ = AssignRole(player, false);
        }
    }
    public static void OnVerified(VerifiedEventArgs ev)
    {
        if (ev.Player.DoNotTrack && ulong.TryParse(ev.Player.RawUserId, out var playerUserID))
        {
            LinkUtils.Unlink(playerUserID);
            return;
        }
        if (!DayLightSyncPlugin.Instance.Config.RoleSync)
            return;
        Timing.CallDelayed(1f,
            () =>
            {
                try
                {
                    _ = AssignRole(ev.Player);
                }
                catch (Exception exception)
                {
                    Log.Error(exception);
                    throw;
                }
            });
    }

    public static async Task<bool> AssignRole(Player player, bool Discord = true)
    {
            var databasePlayer = player.GetDatabasePlayer();
            if (databasePlayer == null)
            {
                Log.Error(player.Nickname + " was not found in Database");
                return false;
            }
            var Time = new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum());
            UserGroup finalgroup = null;
            var HighestPriorty = 0;
            var overridable = false;
            var groupstring = "";

            DayLightSyncConfig.PlayTimeRole FinalRole = default;

            var rolelist = DayLightSyncPlugin.Instance?.Config.Ranks.OrderBy(x => x.Priority);
            if (rolelist == null) return false;
            foreach (var playTimeRole in rolelist)
            {

                var group = ServerStatic.PermissionsHandler.GetGroup(playTimeRole.RankName);
                if (group == null)
                {
                    Log.Error(playTimeRole.RankName + " was not found");
                    continue;
                }



                if (player.Group != null && player.Group.BadgeText == group.BadgeText)
                {
                    overridable = true;
                }

                if (player.Group == null)
                {
                    overridable = true;
                }
                if (!(Time.TotalMinutes > playTimeRole.RequiredMinutes)) continue;
                if (playTimeRole.Priority < HighestPriorty) continue;
                FinalRole = playTimeRole;
                finalgroup = group;
                HighestPriorty = playTimeRole.Priority;
                groupstring = playTimeRole.RankName;
            }

            if (finalgroup != null && overridable)
            {
                player.Group = finalgroup;
                if (!Discord) return true;

                ulong.TryParse(player.RawUserId, out var iResult);
                if (!LinkUtils.IsSteam64Linked(iResult)) return true;
                var data = new RoleSyncHelper
                    { RoleName = groupstring, UserID = LinkUtils.GetLinkedUserID(iResult), RoleID = FinalRole.DiscordRankID, Overrideables = OverridbleRoles()};

                if (DayLightSyncPlugin.Instance.Network.IsConnected)
                    await DayLightSyncPlugin.Instance.Network.SendAsync(new PluginMessage(MessageType.RoleUpdate, data, player.Nickname));

                return true;
            }

            if (overridable) return false;
            Logger.Debug(
                player.Nickname + " group " + player.ReferenceHub.serverRoles.Group.BadgeText + " was not overridable");
        
        return false;
    }

    public static List<ulong>? OverridbleRoles()
    {
        return DayLightSyncPlugin.Instance?.Config.Ranks.Select(x => x.DiscordRankID).ToList();
    }
}
