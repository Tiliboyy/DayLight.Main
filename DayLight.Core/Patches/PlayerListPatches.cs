﻿using CentralAuth;
using DayLight.Core.API.Features;
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
      var args = data.Split(' ');
      if (args.Length != 3 || !int.TryParse(args[0], out var result1) || !int.TryParse(args[1], out var result2) || !Enum.IsDefined(typeof (RaPlayerList.PlayerSorting), (object) result2))
        return false;
      var logToConsole = result1 == 1;
      var descending = !args[2].Equals("1");
      var sortingType = (RaPlayerList.PlayerSorting) result2;
      var viewHiddenBadges = CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenBadges);
      var viewHiddenGlobalBadges = CommandProcessor.CheckPermissions(sender, PlayerPermissions.ViewHiddenGlobalBadges);
      var stringBuilder = StringBuilderPool.Shared.Rent("\n");
      GenerateCategory(CustomRaCategory.RemoteAdminCategories.Where(x => x.AbovePlayers).ToList(), descending, sortingType, stringBuilder, viewHiddenBadges, viewHiddenGlobalBadges);

      GeneratePlayerList(__instance, descending, sortingType, stringBuilder, viewHiddenBadges, viewHiddenGlobalBadges);
      GenerateCategory(CustomRaCategory.RemoteAdminCategories.Where(x => x.AbovePlayers).ToList(), !descending, sortingType, stringBuilder, viewHiddenBadges, viewHiddenGlobalBadges);

      sender.RaReply($"${__instance.DataId} {StringBuilderPool.Shared.ToStringReturn(stringBuilder)}", true, !logToConsole, string.Empty);

      return false;
    }
    private static void GeneratePlayerList(RaPlayerList __instance, bool descending, RaPlayerList.PlayerSorting sortingType, StringBuilder stringBuilder, bool viewHiddenBadges, bool viewHiddenGlobalBadges)
    {
      var removedplayers = CustomRaCategory.RemoteAdminCategories
        .Where(x => x.RemovePlayersFromDefaultList)
        .SelectMany(category => category.Players)
        .Distinct()
        .ToList();
      foreach (var hub in descending ? SortPlayersDescending(sortingType,
                 ReferenceHub.AllHubs.Where(x => !removedplayers.Contains(x)).ToList()) : 
                 SortPlayers(sortingType, ReferenceHub.AllHubs.Where(x => !removedplayers.Contains(x)).ToList()))
      {
        switch (hub.Mode)
        {
          case ClientInstanceMode.Unverified:
          case ClientInstanceMode.DedicatedServer:
            continue;
          default:
            var num2 = hub.serverRoles.IsInOverwatch ? 1 : 0;
            var flag2 = VoiceChatMutes.IsMuted(hub);
            stringBuilder.Append(RaPlayerList.GetPrefix(hub, viewHiddenBadges, viewHiddenGlobalBadges));
            if (num2 != 0)
              stringBuilder.Append("<link=RA_OverwatchEnabled><color=white>[</color><color=#03f8fc>\uF06E</color><color=white>]</color></link> ");
            if (flag2)
              stringBuilder.Append("<link=RA_Muted><color=white>[</color>\uD83D\uDD07<color=white>]</color></link> ");
            var ply = AdvancedPlayer.Get(hub);
            if (ply != null && ply.CustomRemoteAdminBadge != "")
            {
              stringBuilder.Append(ply.CustomRemoteAdminBadge);
            }

            stringBuilder.Append("<color={RA_ClassColor}>(").Append(hub.PlayerId).Append(") ");
            stringBuilder.Append(hub.nicknameSync.CombinedName.Replace("\n", string.Empty).Replace("RA_", string.Empty)).Append("</color>");
            stringBuilder.AppendLine();
            continue;
        }
      }
    }
    private static void GenerateCategory(IEnumerable<CustomRaCategory> categoryManagers, bool descending, RaPlayerList.PlayerSorting sortingType, StringBuilder stringBuilder, bool viewHiddenBadges, bool viewHiddenGlobalBadges)
    {
      foreach (var categoryManager in categoryManagers)
      {
        foreach (var hub in !descending ? SortPlayersDescending(sortingType, categoryManager.Players) : SortPlayers(sortingType, categoryManager.Players.ToList()))
        {
          stringBuilder.Append($"<align=center><size=0>({categoryManager.Id})</size> <size={categoryManager.Size}></color><color={categoryManager.Color}>{categoryManager.Name}</color></size>\n</align>");

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
              if (num2 != 0) stringBuilder.Append("<link=RA_OverwatchEnabled><color=white>[</color><color=#03f8fc>\uF06E</color><color=white>]</color></link> ");
              if (flag2) stringBuilder.Append("<link=RA_Muted><color=white>[</color>\uD83D\uDD07<color=white>]</color></link> ");
              var ply = AdvancedPlayer.Get(hub);
              if (ply != null && ply.CustomRemoteAdminBadge != "")
              {
                stringBuilder.Append(ply.CustomRemoteAdminBadge);
              }
              stringBuilder.Append("<color={RA_ClassColor}>(").Append(hub.PlayerId).Append(") ");
              stringBuilder.Append(hub.nicknameSync.CombinedName.Replace("\n", string.Empty).Replace("RA_", string.Empty)).Append("</color>");
              stringBuilder.AppendLine();
              continue;
          }
        }

      }
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
