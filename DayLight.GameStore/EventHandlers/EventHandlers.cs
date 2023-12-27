using DayLight.Core.API;
using DayLight.Core.API.Database;
using DayLight.Core.API.Features;
using Discord;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System;
using System.Collections.Generic;
using DamageType = Exiled.API.Enums.DamageType;
using Player = Exiled.API.Features.Player;
using Round = Exiled.API.Features.Round;

namespace DayLight.GameStore.EventHandlers;

public class EventHandlers
{
    public static Dictionary<Player, Player> PocketPlayers = new();

    public static void OnVerified(VerifiedEventArgs ev)
    {
    }
    
    public static void OnGainingLevel(Exiled.Events.EventArgs.Scp079.GainingLevelEventArgs ev)
    {
        ev.Player?.GiveGameStoreReward(GameStorePlugin.Instance.Config.Scp079LevelGameStoreReward);
    }
    public static void OnEscaping(EscapingEventArgs ev)
    {
        ev.Player?.GiveGameStoreReward(GameStorePlugin.Instance.Config.EscapeGameStoreReward);
        ev.Player?.Cuffer?.GiveGameStoreReward(GameStorePlugin.Instance.Config.CufferGameStoreReward);
    }
    
    public static void OnUsedItem(UsedItemEventArgs ev)
    {
        if(ev.Item.Type == ItemType.Medkit && ev.Player.Health == ev.Player.MaxHealth) return;
        ev.Player?.GiveGameStoreReward(GameStorePlugin.Instance.Config.UsingItemGameStoreReward);
    }
    
    public static void OnSpawned(SpawnedEventArgs ev)
    {
        if(ev.Player == null) return;

        var adv = AdvancedPlayer.Get(ev.Player);
        if(adv == null)
            Logger.Error("NULL");
        if (ev.Reason is SpawnReason.Respawn or SpawnReason.RoundStart or SpawnReason.LateJoin)
            ev.Player?.GiveGameStoreReward(GameStorePlugin.Instance.Config.SpawnGameStoreReward);
    }
    
    public static void OnThownItem(ThrownProjectileEventArgs ev)
    {
        ev.Player?.GiveGameStoreReward(GameStorePlugin.Instance.Config.UsingItemGameStoreReward);
    }
    
    public static void OnDying(DyingEventArgs ev)
    {
        if(Round.IsEnded || !Round.IsStarted) return;
        if (ev.Player == null) 
            return;
        ev.Player.GiveGameStoreReward(GameStorePlugin.Instance.Config.DeathGameStoreReward);
        if (ev.DamageHandler.Type == DamageType.PocketDimension)
        {
            if (!PocketPlayers.ContainsKey(ev.Player)) return; 
            PocketPlayers[ev.Player].GiveGameStoreReward(GameStorePlugin.Instance.Config.KillGameStoreReward);
            PocketPlayers.Remove(ev.Player);
            return;
        }
        if (PocketPlayers.ContainsKey(ev.Player))
            PocketPlayers.Remove(ev.Player);
        
        if (ev.Attacker == null)
            return;
        if (ev.Player.Role.Team == Team.SCPs && ev.Player.Role.Type != RoleTypeId.Scp0492)
            ev.Attacker.GiveGameStoreReward(GameStorePlugin.Instance.Config.ScpKillGameStoreReward);
        else
            ev.Attacker.GiveGameStoreReward(GameStorePlugin.Instance.Config.KillGameStoreReward);

    }
    
    public static void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
    {
        if(ev.Scp106 != null && ev.Player != null && PocketPlayers.ContainsKey(ev.Player))
            PocketPlayers.Add(ev.Player, ev.Scp106);
    }
    
    public static void OnFailingEscapePocketDimension(FailingEscapePocketDimensionEventArgs ev)
    {
        if (!PocketPlayers.ContainsKey(ev.Player)) return;
        PocketPlayers[ev.Player].GiveGameStoreReward(GameStorePlugin.Instance.Config.KillGameStoreReward);
        PocketPlayers.Remove(ev.Player);
    }
    
    public static void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
    {
        if (PocketPlayers.ContainsKey(ev.Player))
            PocketPlayers.Remove(ev.Player);
    }
}