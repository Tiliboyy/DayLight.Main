using CentralAuth;
using DayLight.Core.API.Events.EventArgs;
using Exiled.API.Features;
using HarmonyLib;
using Mirror;
using Neuron.Core.Meta;
using NorthwoodLib.Pools;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using RemoteAdmin;
using RemoteAdmin.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using VoiceChat;

namespace DayLight.Core.Patches;

[Automatic]
[HarmonyPatch]
public static class RaPlayerPatch
{
    [HarmonyPatch(typeof(RaPlayer), nameof(RaPlayer.ReceiveData), typeof(CommandSender), typeof(string))] 
    public static bool Prefix(RaPlayer __instance, CommandSender sender, string data)
    {
      var source = data.Split(' ');
      if (source.Length != 2 || !int.TryParse(source[0], out var result))
        return false;
      var flag1 = result == 1;
      var playerCommandSender1 = sender as PlayerCommandSender;
      if (!flag1 && playerCommandSender1 != null && !playerCommandSender1.ReferenceHub.authManager.RemoteAdminGlobalAccess && !playerCommandSender1.ReferenceHub.authManager.BypassBansFlagSet && !CommandProcessor.CheckPermissions(sender, PlayerPermissions.PlayerSensitiveDataAccess))
        return false;
      var referenceHubList = RAUtils.ProcessPlayerIdOrNamesList(new ArraySegment<string>(source.Skip(1).ToArray()), 0, out var _);
      if (referenceHubList.Count == 0)
        return false;
      var flag2 = PermissionsHandler.IsPermitted(sender.Permissions, 18007046UL);
      if (playerCommandSender1 != null && playerCommandSender1.ReferenceHub.authManager.NorthwoodStaff)
        flag2 = true;
      if (referenceHubList.Count > 1)
      {
        var stringBuilder = StringBuilderPool.Shared.Rent("<color=white>");
        stringBuilder.Append("Selecting multiple players:");
        stringBuilder.Append("\nPlayer ID: <color=green><link=CP_ID>\uF0C5</link></color>");
        stringBuilder.Append("\nIP Address: " + (!flag1 ? "<color=green><link=CP_IP>\uF0C5</link></color>" : "[REDACTED]"));
        stringBuilder.Append("\nUser ID: " + (flag2 ? "<color=green><link=CP_USERID>\uF0C5</link></color>" : "[REDACTED]"));
        stringBuilder.Append("</color>");
        var data1 = string.Empty;
        var data2 = string.Empty;
        var data3 = string.Empty;
        foreach (var referenceHub in referenceHubList)
        {
          data1 = data1 + referenceHub.PlayerId.ToString() + ".";
          if (!flag1)
            data2 = data2 + (referenceHub.networkIdentity.connectionToClient.IpOverride != null ? referenceHub.networkIdentity.connectionToClient.OriginalIpAddress : referenceHub.networkIdentity.connectionToClient.address) + ",";
          if (flag2)
            data3 = data3 + referenceHub.authManager.UserId + ".";
        }
        if (data1.Length > 0)
          RaClipboard.Send(sender, RaClipboard.RaClipBoardType.PlayerId, data1);
        if (data2.Length > 0)
          RaClipboard.Send(sender, RaClipboard.RaClipBoardType.Ip, data2);
        if (data3.Length > 0)
          RaClipboard.Send(sender, RaClipboard.RaClipBoardType.UserId, data3);
        sender.RaReply($"${__instance.DataId} {stringBuilder}", true, true, string.Empty);
        StringBuilderPool.Shared.Return(stringBuilder);
      }
      else
      {
        var referenceHub = referenceHubList[0];
        ServerLogs.AddLog(ServerLogs.Modules.DataAccess, $"{sender.LogName} accessed IP address of player {referenceHub.PlayerId} ({referenceHub.nicknameSync.MyNick}).", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
        var flag3 = PermissionsHandler.IsPermitted(sender.Permissions, PlayerPermissions.GameplayData);
        var characterClassManager = referenceHub.characterClassManager;
        var authManager = referenceHub.authManager;
        var nicknameSync = referenceHub.nicknameSync;
        var connectionToClient = referenceHub.networkIdentity.connectionToClient;
        var serverRoles = referenceHub.serverRoles;
        if (sender is PlayerCommandSender playerCommandSender2)
          playerCommandSender2.ReferenceHub.queryProcessor.GameplayData = flag3;
        var stringBuilder = StringBuilderPool.Shared.Rent("<color=white>");
        stringBuilder.Append("Nickname: " + nicknameSync.CombinedName);
        stringBuilder.Append($"\nPlayer ID: {referenceHub.PlayerId} <color=green><link=CP_ID>\uF0C5</link></color>");
        RaClipboard.Send(sender, RaClipboard.RaClipBoardType.PlayerId, $"{referenceHub.PlayerId}");
        if (connectionToClient == null)
          stringBuilder.Append("\nIP Address: null");
        else if (!flag1)
        {
          stringBuilder.Append("\nIP Address: " + connectionToClient.address + " ");
          if (connectionToClient.IpOverride != null)
          {
            RaClipboard.Send(sender, RaClipboard.RaClipBoardType.Ip, connectionToClient.OriginalIpAddress ?? "");
            stringBuilder.Append(" [routed via " + connectionToClient.OriginalIpAddress + "]");
          }
          else
            RaClipboard.Send(sender, RaClipboard.RaClipBoardType.Ip, connectionToClient.address ?? "");
          stringBuilder.Append(" <color=green><link=CP_IP>\uF0C5</link></color>");
        }
        else
          stringBuilder.Append("\nIP Address: [REDACTED]");
        stringBuilder.Append("\nUser ID: " + (flag2 ? (string.IsNullOrEmpty(authManager.UserId) ? "(none)" : authManager.UserId + " <color=green><link=CP_USERID>\uF0C5</link></color>") : "<color=#D4AF37>INSUFFICIENT PERMISSIONS</color>"));
        if (flag2)
        {
          RaClipboard.Send(sender, RaClipboard.RaClipBoardType.UserId, authManager.UserId ?? "");
          if (authManager.SaltedUserId != null && authManager.SaltedUserId.Contains("$"))
            stringBuilder.Append("\nSalted User ID: " + authManager.SaltedUserId);
        }
        stringBuilder.Append("\nServer role: " + serverRoles.GetColoredRoleString());
        var flag4 = CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenBadges);
        var flag5 = CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenGlobalBadges);
        if (playerCommandSender1 != null)
        {
          flag4 = true;
          flag5 = true;
        }
        var flag6 = !string.IsNullOrEmpty(serverRoles.HiddenBadge);
        var flag7 = !flag6 || serverRoles.GlobalHidden & flag5 || !serverRoles.GlobalHidden & flag4;
        if (flag7)
        {
          if (flag6)
          {
            stringBuilder.Append("\n<color=#DC143C>Hidden role: </color>" + serverRoles.HiddenBadge);
            stringBuilder.Append("\n<color=#DC143C>Hidden role type: </color>" + (serverRoles.GlobalHidden ? "GLOBAL" : "LOCAL"));
          }
          if (referenceHub.authManager.RemoteAdminGlobalAccess)
            stringBuilder.Append("\nStudio Status: <color=#BCC6CC>Studio GLOBAL Staff (management or global moderation)</color>");
          else if (referenceHub.authManager.NorthwoodStaff)
            stringBuilder.Append("\nStudio Status: <color=#94B9CF>Studio Staff</color>");
        }
        var flags = VoiceChatMutes.GetFlags(referenceHubList[0]);
        if (flags != VcMuteFlags.None)
        {
          stringBuilder.Append("\nMUTE STATUS:");
          foreach (var vcMuteFlags in EnumUtils<VcMuteFlags>.Values)
          {
            if (vcMuteFlags == VcMuteFlags.None || (flags & vcMuteFlags) != vcMuteFlags) continue;
            stringBuilder.Append(" <color=#F70D1A>");
            stringBuilder.Append(vcMuteFlags);
            stringBuilder.Append("</color>");
          }
        }
        stringBuilder.Append("\nActive flag(s):");
        if (characterClassManager.GodMode)
          stringBuilder.Append(" <color=#659EC7>[GOD MODE]</color>");
        if (referenceHub.playerStats.GetModule<AdminFlagsStat>().HasFlag(AdminFlags.Noclip))
          stringBuilder.Append(" <color=#DC143C>[NOCLIP ENABLED]</color>");
        else if (FpcNoclip.IsPermitted(referenceHub))
          stringBuilder.Append(" <color=#E52B50>[NOCLIP UNLOCKED]</color>");
        if (authManager.DoNotTrack)
          stringBuilder.Append(" <color=#BFFF00>[DO NOT TRACK]</color>");
        if (serverRoles.BypassMode)
          stringBuilder.Append(" <color=#BFFF00>[BYPASS MODE]</color>");
        if (flag7 && serverRoles.RemoteAdmin)
          stringBuilder.Append(" <color=#43C6DB>[RA AUTHENTICATED]</color>");
        if (serverRoles.IsInOverwatch)
          stringBuilder.Append(" <color=#008080>[OVERWATCH MODE]</color>");
        else if (flag3)
        {
          stringBuilder.Append("\nClass: ").Append(PlayerRoleLoader.AllRoles.TryGetValue(referenceHub.GetRoleId(), out var playerRoleBase) ? playerRoleBase.RoleTypeId : "None");
          stringBuilder.Append(" <color=#fcff99>[HP: ").Append(CommandProcessor.GetRoundedStat<HealthStat>(referenceHub)).Append("]</color>");
          stringBuilder.Append(" <color=green>[AHP: ").Append(CommandProcessor.GetRoundedStat<AhpStat>(referenceHub)).Append("]</color>");
          stringBuilder.Append(" <color=#977dff>[HS: ").Append(CommandProcessor.GetRoundedStat<HumeShieldStat>(referenceHub)).Append("]</color>");
          stringBuilder.Append("\nPosition: ").Append(referenceHub.transform.position.ToPreciseString());
        }
        else
          stringBuilder.Append("\n<color=#D4AF37>Some fields were hidden. GameplayData permission required.</color>");
        stringBuilder.Append("</color>");
        var eventargs = new RequestingPlayerDataEventArgs(Player.Get(sender), Player.Get(referenceHub), StringBuilderPool.Shared.ToStringReturn(stringBuilder),true);
        API.Events.Handlers.RemoteAdmin.OnRequestingData(eventargs);
        if (!eventargs.IsAllowed) return false;
        sender.RaReply($"${__instance.DataId} {StringBuilderPool.Shared.ToStringReturn(stringBuilder)}", true, true, string.Empty);
        RaPlayerQR.Send(sender, false, string.IsNullOrEmpty(authManager.UserId) ? "(no User ID)" : authManager.UserId);
        
      }
      return false;
    }

}
