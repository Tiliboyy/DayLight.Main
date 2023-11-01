using CentralAuth;
using DayLight.Core.API.Features;
using Exiled.API.Features;
using HarmonyLib;
using Neuron.Core.Meta;
using NorthwoodLib.Pools;
using PlayerRoles;
using RemoteAdmin;
using RemoteAdmin.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoiceChat;

namespace DayLight.Core.Patches;

[Automatic]
[HarmonyPatch]
public static class RemoteAdminListPatch
{ 
    [HarmonyPatch(typeof(RaPlayerList), nameof(RaPlayerList.ReceiveData), typeof(CommandSender), typeof(string))]
    public static bool Prefix(RaPlayerList __instance, CommandSender sender, string data)
    {
      var strArray = data.Split(' ');
      if (strArray.Length != 3 || !int.TryParse(strArray[0], out var result1) || !int.TryParse(strArray[1], out var result2) || !Enum.IsDefined(typeof (RaPlayerList.PlayerSorting), result2))
        return false;
      var flag1 = result1 == 1;
      var num1 = strArray[2].Equals("1") ? 1 : 0;
      var sortingType = (RaPlayerList.PlayerSorting) result2;
      var viewHiddenBadges = CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenBadges);
      var viewHiddenGlobalBadges = CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenGlobalBadges);
      var stringBuilder = StringBuilderPool.Shared.Rent("\n");
      foreach (var hub in num1 != 0 ? SortPlayersDescending(sortingType, ReferenceHub.AllHubs.ToList()) : SortPlayers(sortingType, ReferenceHub.AllHubs.ToList()))
      {
        switch (hub.Mode)
        {
          case ClientInstanceMode.Unverified:
          case ClientInstanceMode.DedicatedServer:
            continue;
          case ClientInstanceMode.ReadyClient:
            break;
          case ClientInstanceMode.Host:
            break;
          default:
            var num2 = hub.serverRoles.IsInOverwatch ? 1 : 0;
            var flag2 = VoiceChatMutes.IsMuted(hub);
            stringBuilder.Append(RaPlayerList.GetPrefix(hub, viewHiddenBadges, viewHiddenGlobalBadges));
            if (num2 != 0)
              stringBuilder.Append("<link=RA_OverwatchEnabled><color=white>[</color><color=#03f8fc>\uF06E</color><color=white>]</color></link> ");
            if (flag2)
              stringBuilder.Append("<link=RA_Muted><color=white>[</color>\uD83D\uDD07<color=white>]</color></link> ");
            stringBuilder.Append("<color={RA_ClassColor}>(").Append(hub.PlayerId).Append(") ");
            stringBuilder.Append(hub.nicknameSync.CombinedName.Replace("\n", string.Empty).Replace("RA_", string.Empty)).Append("</color>");
            stringBuilder.AppendLine();
            continue;
        }
      }
      sender.RaReply($"${__instance.DataId} {StringBuilderPool.Shared.ToStringReturn(stringBuilder)}", true, !flag1, string.Empty);
      return false;
    }
    
      public static IEnumerable<ReferenceHub> SortPlayers(RaPlayerList.PlayerSorting sortingType, List<ReferenceHub> hubs)
    {
      switch (sortingType)
      {
        case RaPlayerList.PlayerSorting.Alphabetical:
          return hubs.OrderBy(h => h.nicknameSync.DisplayName ?? h.nicknameSync.MyNick);
        case RaPlayerList.PlayerSorting.Class:
          return hubs.OrderBy(h => h.GetTeam()).ThenBy(h => h.GetRoleId());
        case RaPlayerList.PlayerSorting.Team:
          return hubs.OrderBy(h => h.roleManager.CurrentRole.Team);
        default:
          return hubs.OrderBy(h => h.PlayerId);
      }
    }

    public static IEnumerable<ReferenceHub> SortPlayersDescending(RaPlayerList.PlayerSorting sortingType, List<ReferenceHub> referenceHubs)
    {
      switch (sortingType)
      {
        case RaPlayerList.PlayerSorting.Alphabetical:
          return referenceHubs.OrderByDescending(h => h.nicknameSync.DisplayName ?? h.nicknameSync.MyNick);
        case RaPlayerList.PlayerSorting.Class:
          return referenceHubs.OrderByDescending(h => h.GetTeam()).ThenByDescending(h => h.GetRoleId());
        case RaPlayerList.PlayerSorting.Team:
          return referenceHubs.OrderByDescending(h => h.roleManager.CurrentRole.Team);
        default:
          return referenceHubs.OrderByDescending(h => h.PlayerId);
      }
    }


}
