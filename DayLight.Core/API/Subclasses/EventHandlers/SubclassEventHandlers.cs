using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomRoles.API;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using JetBrains.Annotations;
using Neuron.Core;
using System.Collections.Generic;
using System.Linq;
using Player = Exiled.API.Features.Player;
using Random = UnityEngine.Random;

namespace DayLight.Core.API.Subclasses.EventHandlers;

public class SubclassEventHandlers 
{
    [UsedImplicitly] public static List<Player> NoRolePlayers = new();

    public static void OnChangingRole(ChangingRoleEventArgs ev)
    {
        var allowedReasons = new List<SpawnReason>
        {
            SpawnReason.Revived,
            SpawnReason.LateJoin
        };
        if (!Globals.Get<SubclassConfig>().AlwaysAllowSpawns && !allowedReasons.Contains(ev.Reason)) return;
        GiveRandomSubclass(ev.Player);
    }

    public static void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        if (Manager.NoRandomRole)
            return;
        var random = new System.Random();
        foreach (var player in ev.Players.OrderBy(_ => random.Next()))
        {
            GiveRandomSubclass(player);
        }
    }
    public static void RoundStarted()
    {
        
        if (Manager.NoRandomRole)
            return;
        var random = new System.Random();
        foreach (var player in Player.List.OrderBy(_ => random.Next()))
        {
            GiveRandomSubclass(player);
        }
    }

    public static void GiveRandomSubclass(Player player, bool ignoreReason = true)
    {
        if (NoRolePlayers.Contains(player))
            return;
        if (player.GetCustomRoles().Count != 0) return;

        var options = Manager.Subclasses.Where(subclassManager => subclassManager.ClassRoleType == player.Role)
            .ToList();
        if (options.Count == 0 || !Round.IsStarted || Manager.NoRoleAllowed.Contains(player)) return;
        var subclasses = options.Where(manager => Chance(manager.ClassChance, manager)).ToList();
        if (subclasses.Count == 0) return;
        var range = Random.Range(0, subclasses.Count);
        var spawned = 0;
        var max = 0;
        foreach (var subclassManager in Manager.Subclasses.Where(subclassManager =>
                     subclassManager.ClassName == subclasses[range].ClassName))
        {
            subclassManager.SpawnedAmount += 1;
            spawned = subclassManager.SpawnedAmount;
            max = subclassManager.MaxSpawnAmount;
        }

        if (spawned > max)
        {
            return;
        }

        var role = Exiled.CustomRoles.API.Features.CustomRole.Get(subclasses[range].ClassName);
        role?.AddRole(player);
        Logger.Debug(player.Nickname + " was grated role: " +
                     Exiled.CustomRoles.API.Features.CustomRole.Get(subclasses[range].ClassName)?.Name +
                     "\nSpawned: " + spawned + "/" + max);
    }

    
    private static bool Chance(int chance, Manager.Subclass subclass)
    {
        var e = Random.Range(1, 100);
        Logger.Info(subclass.ClassName + " " + $"{e}/{subclass.ClassChance} ({e <= chance})");
        return e <= chance;
    }

    public static void OnRoundEnd(RoundEndedEventArgs ev)
    {
        foreach (var subclass in Manager.Subclasses)
            subclass.SpawnedAmount = 0;
    }
}