﻿#region

using DayLight.Core;
using DayLight.Core.API;
using DayLight.Core.API.Database;
using DayLight.Stat.Achievements;
using DayLight.Stat.Stats.Components;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using System;
using System.Linq;
using Map = Exiled.Events.Handlers.Map;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

#endregion

namespace DayLight.Stat.Stats;

internal class StatsEventHandler
{
    private static bool HasEnded;
    public static void RegisterEvents()
    {
        Player.UsedItem += OnUsingItem;
        Player.Escaping += OnEscaping;
        Player.Died += OnDeath;
        Player.Verified += OnVerified;
        Server.EndingRound += OnEndingRound;
        Map.ExplodingGrenade += OnExploding;
    }
    public static void UnRegisterEvents()
    {
        Player.UsedItem -= OnUsingItem;
        Player.Escaping -= OnEscaping;
        Player.Died -= OnDeath;
        Player.Verified -= OnVerified;
        Server.EndingRound -= OnEndingRound;
        Map.ExplodingGrenade -= OnExploding;
    }

    public static void OnDeath(DiedEventArgs ev)
    {
        if (RoleExtensions.GetTeam(ev.TargetOldRole) == Team.SCPs && ev.TargetOldRole != RoleTypeId.Scp0492)
        {
            ev.Attacker.GetAdvancedPlayer().DatabasePlayer.Stats.KilledScps += 1;
        }
        ev.Player.GetAdvancedPlayer().DatabasePlayer.Stats.Deaths += 1;
        ev.Attacker.GetAdvancedPlayer().DatabasePlayer.Stats.Kills += 1;

    }

    public static void OnUsingItem(UsedItemEventArgs ev)
    {
        var item = Enum.TryParse<Dependencys.Utils.ItemType>(ev.Item.Type.ToString(), out var e);
        if (ev.Player.GetAdvancedPlayer().DatabasePlayer.Stats.UsedItems.ContainsKey(e))
        {
            ev.Player.GetAdvancedPlayer().DatabasePlayer.Stats.UsedItems[e] += 1;
        }
        else
        {
            ev.Player.GetAdvancedPlayer().DatabasePlayer.Stats.UsedItems.Add(e, 1);
        }
    }

    public static void OnEscaping(EscapingEventArgs ev)
    {
        if (Round.ElapsedTime.TotalSeconds <= 120) ev.Player.Achive(37);
        ev.Player.GetAdvancedPlayer().DatabasePlayer.Stats.FastestEscapeSeconds = Round.ElapsedTime.TotalSeconds;
    }

    public static void OnExploding(ExplodingGrenadeEventArgs ev)
    {
        if (ev.Projectile.GameObject.TryGetComponent<PinkCandyComponent>(out var component))
        {
            if (ev.TargetsToAffect.Count - 1 >= 5)
            {
                ev.Player.Achive(2);

            }
            if (ev.TargetsToAffect.Count(X => X.Role.Team == Team.SCPs) >= 3)
                ev.Player.Achive(30);
            ev.Player.GetAdvancedPlayer().DatabasePlayer.Stats.PinkCandyKills += 1;
            component.DestroyComponent();
        }
        else
        {
            if (ev.Projectile.Type != ItemType.GrenadeHE) return;
            if (ev.TargetsToAffect.Count(x => x.Role.Team == Team.SCPs) >= 3)
                ev.Player.Achive(32);
        }
    }
    public static void OnEndingRound(EndingRoundEventArgs ev)
    {
        if (HasEnded) return;
        HasEnded = true;
        foreach (var player in Exiled.API.Features.Player.List)
        {
            player.GetAdvancedPlayer().DatabasePlayer.Stats.PlayedRounds += 1;
        }
        GameStateData.ClearGameStats();
        Leaderboard.UpdateLeaderboards();
    }


    public static void OnVerified(VerifiedEventArgs ev)
    {
    }
    
}
