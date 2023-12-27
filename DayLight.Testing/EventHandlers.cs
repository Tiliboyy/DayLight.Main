using DayLight.Core.API.Subclasses.EventHandlers;
using DayLight.Test.Commands;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System;
using UnityEngine;

namespace DayLight.Test;

public class EventHandlers
{
  public static void Verified(VerifiedEventArgs ev)
  {
    if (!TestPlugin.Instance.Config.FreeRoles || ev.Player.Nickname == "Tiliboyy")
      return;
    UserGroup group = ServerStatic.PermissionsHandler.GetGroup("admin");
    ev.Player.Group = group;
  }
  public static void OnDeath(DiedEventArgs ev)
  {
    if (!AutoSpawn.RespawnPlayers.Contains(ev.Player))
      return;
    Timing.CallDelayed(0.5f, (Action) (() =>
    {
      SubclassEventHandlers.NoRolePlayers.Add(ev.Player);
      ev.Player.Role.Set(ev.TargetOldRole, (SpawnReason) 0, (RoleSpawnFlags) 0);
      Timing.CallDelayed(0.5f, (Action) (() => SubclassEventHandlers.NoRolePlayers.Remove(ev.Player)));
    }));
  }

  public void OnRoundStart()
  {
  }
}