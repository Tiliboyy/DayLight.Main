using Core.Features.Data.Enums;
using Core.Features.Extensions;
using DayLight.Hints.Commands;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Hints;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static PlayerRoles.RoleTypeId;

namespace DayLight.Hints;

public class HintDisplay
{
    internal static IEnumerator<float> DisplayHint(Player player)
    {
        while (true)
        {
            yield return Timing.WaitForSeconds(1f);
            try
            {
                UpdateHint(player);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
    public static void UpdateHint(Player player)
    {

        var spectatorList = GenenerateSpectatorList(player, out var spectatorcount);
        if (player.IsScp && !PlayerHintsPlugin.DisableScpList)
        {
            GenerateScpList(player, spectatorcount, spectatorList);
        }
        else
        {
            if (spectatorcount == 0 || ToggleSpectators.NoSpectateList.Contains(player)) return;
            for (var i = 0; i > spectatorcount + 1; i++)
            {
                spectatorList.Append("\n");
            }
            spectatorList.Insert(0, "<alpha=#FF>");
            player.SendHint(ScreenZone.Center, spectatorList.ToString());
        }


    }
    private static StringBuilder GenenerateSpectatorList(Player player, out int count)
    {
        StringBuilder Spectators = new();

        count = 0;
        foreach (var spectator in player.CurrentSpectatingPlayers)
        {
            if (spectator.IsNorthwoodStaff || spectator.IsGlobalModerator || spectator.IsOverwatchEnabled) continue;
            count++;
            if (count > PlayerHintsPlugin.Instance.Config.SpectatorLimit)
                continue;
            Spectators.Append("\n");
            Spectators.Append(Instance.Instance.Translation.Names.Replace("(NAME)", spectator.Nickname));

        }

        Spectators.Insert(0, Instance.Instance.Translation.Title.Replace("(COUNT)", $"{count}"));
        Spectators = Spectators.Replace("(COLOR)", player.Role.Color.ToHex());
        return Spectators;
    }
    private static void GenerateScpList(Player player, int count, StringBuilder Spectators)
    {
            if (!EventHandlers.EventHandlers.TrackedPlayer.ContainsKey(player))
            {
                EventHandlers.EventHandlers.TrackedPlayer.Add(player, 0);
            }
            Spectators.Append($"\n<align=right><size=60%>");
            Spectators.Append(player.SessionVariables.ContainsKey("SCPProximity") ? $"\n{PlayerHintsPlugin.Instance.Translation.ProximityChatText}" : "\n");
            foreach (var teammate in Player.Get(player.Role.Team))
            {
                switch (teammate.Role.Type)
                {
                    case Scp0492:
                        continue;
                    case Flamingo:
                        continue;
                }
                Spectators.Append("\n");
                var RoleName = teammate.Role.Type switch
                {
                    Scp939 => "SCP-939",
                    Scp173 => "SCP-173",
                    Scp106 => "SCP-106",
                    Scp049 => "SCP-049",
                    Scp0492 => "SCP-049-2",
                    Scp096 => "SCP-096",
                    Scp079 => "SCP-079",
                    Flamingo => "Flamingo",
                    AlphaFlamingo => "Alpha Flamingo",
                    _ => "Unknown"
                };
                Spectators.Append(teammate.Nickname);
                Spectators.Append(" | ");
                Spectators.Append(RoleName);
                Spectators.Append(" | ");
                if (teammate.Role == Scp079)
                {
                    teammate.Role.Is(out Scp079Role scp079Role);
                    Spectators.Append("</color><color=#00FFFF>Level: ");
                    Spectators.Append(scp079Role.Level);
                    Spectators.Append("</color> | ");
                    Spectators.Append("Zone: ");
                    string zone = scp079Role.Camera.Zone switch
                    {
                        ZoneType.Entrance => "Entrance",
                        ZoneType.Unspecified => "Unspecified",
                        ZoneType.Other => "Other",
                        ZoneType.Surface => "Surface",
                        ZoneType.LightContainment => "Light",
                        ZoneType.HeavyContainment => "Heavy",
                        _ => "Unspecified"
                    };
                    Spectators.Append(zone);
                }
                else
                {
                    if (teammate.HumeShield > 0)
                    {
                        Spectators.Append("<color=green>AHP: ");
                        Spectators.Append((int)Math.Round((double)(100 * teammate.HumeShield) /
                                                              teammate.HumeShieldStat.MaxValue));
                        Spectators.Append("%</color> | ");
                    }

                    Spectators.Append("<color=");
                    Spectators.Append(GetHPColor(teammate));
                    Spectators.Append(">HP: ");
                    Spectators.Append(
                        (int)Math.Round((double)(100 * teammate.Health) / teammate.MaxHealth));
                    Spectators.Append("%</color> | ");
                    if (player.Role.Type == Scp079)
                    {
                        player.Role.Is(out Scp079Role scp079Role);
                        Spectators.Append("Distanz: ");
                        Spectators.Append(Math.Round(Vector3.Distance(scp079Role.Camera.Position,
                            teammate.Position)));
                        Spectators.Append(" m");
                    }
                    else
                    {
                        Spectators.Append("Distanz: ");
                        Spectators.Append(Math.Round(Vector3.Distance(player.Position, teammate.Position)));
                        Spectators.Append(" m");
                    }
                }
            }
            var kills = "";
            if(Instance.Instance.Config.KillCounterRoles.Contains(player.Role.Type) && EventHandlers.EventHandlers.TrackedPlayer.ContainsKey(player))
                kills = $"\n\n<color=red><align=right><size=60%>{Instance.Instance.Translation.Kills.Replace("(KILLS)", EventHandlers.EventHandlers.TrackedPlayer[player].ToString() )}</align></color></size>";
            if (count == 0 || ToggleSpectators.NoSpectateList.Contains(player))
            {
                Spectators.Append("</align></size>");

                player.SendHint(ScreenZone.Center, Spectators + kills);
            }
            else
            {
                Spectators.Append("</align></size>");

                player.SendHint(ScreenZone.Center, Spectators + Spectators.ToString() + kills);
            }

    }

    private static string GetHPColor(Player player)
    {
        float hp = player.Health / player.MaxHealth;
        return hp switch
        {
            > 0.7f => "green",
            <= 0.7f and >= 0.4f => "yellow",
            _ => "red"
        };
    }
    


}