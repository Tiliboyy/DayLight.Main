using DayLight.Core;
using DayLight.Core.API;
using DayLight.Core.API.Database;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Enums;
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
        if (ev.Player.DoNotTrack)
        {
            DayLightDatabase.RemovePlayer(ev.Player);
            return;
        }
        DayLightDatabase.AddPlayer(ev.Player);
    }
    
    public static void OnGainingLevel(Exiled.Events.EventArgs.Scp079.GainingLevelEventArgs ev)
    {
        ev.Player?.GiveGameStoreReward(GameStorePlugin.Instance.Config.Scp079LevelReward);
    }
    public static void OnEscaping(EscapingEventArgs ev)
    {
        ev.Player?.GiveGameStoreReward(GameStorePlugin.Instance.Config.EscapeReward);
        ev.Player?.Cuffer?.GiveGameStoreReward(GameStorePlugin.Instance.Config.CufferReward);
    }
    
    public static void OnUsedItem(UsedItemEventArgs ev)
    {
        if(ev.Item.Type == ItemType.Medkit && ev.Player.Health == ev.Player.MaxHealth) return;
        ev.Player?.GiveGameStoreReward(GameStorePlugin.Instance.Config.UsingItemReward);
    }
    
    public static void OnSpawned(SpawnedEventArgs ev)
    {
        if(ev.Player == null) return;
        if (ev.Reason is SpawnReason.Respawn or SpawnReason.RoundStart or SpawnReason.LateJoin)
            ev.Player?.GiveGameStoreReward(GameStorePlugin.Instance.Config.SpawnReward);
    }
    
    public static void OnThownItem(ThrownProjectileEventArgs ev)
    {
        ev.Player?.GiveGameStoreReward(GameStorePlugin.Instance.Config.UsingItemReward);
    }
    
    public static void OnDying(DyingEventArgs ev)
    {
        if(Round.IsEnded || !Round.IsStarted) return;
        if (ev.Player == null) 
            return;
        ev.Player.GiveGameStoreReward(GameStorePlugin.Instance.Config.DeathReward);
        if (ev.DamageHandler.Type == DamageType.PocketDimension)
        {
            if (!PocketPlayers.ContainsKey(ev.Player)) return; 
            PocketPlayers[ev.Player].GiveGameStoreReward(GameStorePlugin.Instance.Config.KillReward);
            PocketPlayers.Remove(ev.Player);
            return;
        }
        if (PocketPlayers.ContainsKey(ev.Player))
            PocketPlayers.Remove(ev.Player);
        
        
        if (ev.Attacker == null || ev.Attacker.Id == ev.Player.Id)
            return;
        if (ev.Player.Role.Team == Team.SCPs && ev.Player.Role.Type != RoleTypeId.Scp0492)
            ev.Attacker.GiveGameStoreReward(GameStorePlugin.Instance.Config.ScpKillReward);
        else
            ev.Attacker.GiveGameStoreReward(GameStorePlugin.Instance.Config.KillReward);

    }
    
    public static void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
    {
        if(ev.Scp106 != null && ev.Player != null && PocketPlayers.ContainsKey(ev.Player))
            PocketPlayers.Add(ev.Player, ev.Scp106);
    }
    
    public static void OnFailingEscapePocketDimension(FailingEscapePocketDimensionEventArgs ev)
    {
        if (!PocketPlayers.ContainsKey(ev.Player)) return;
        PocketPlayers[ev.Player].GiveGameStoreReward(GameStorePlugin.Instance.Config.KillReward);
        PocketPlayers.Remove(ev.Player);
    }
    
    public static void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
    {
        if (PocketPlayers.ContainsKey(ev.Player))
            PocketPlayers.Remove(ev.Player);
    }
}