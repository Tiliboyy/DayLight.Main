using DayLight.Core.API.Database;
using DayLight.Dependency.Models;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using JetBrains.Annotations;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Warhead = PluginAPI.Core.Warhead;

namespace DayLight.Core.API.Features;

public class AdvancedPlayer : MonoBehaviour
{
  public Dictionary<int, int> GameStoreBoughtItems = new();
  public Dictionary<string, int> GameStoreRewardLimit = new();
  public void ResetGameStoreLimits()
  {
    GameStoreBoughtItems = new Dictionary<int, int>();
    GameStoreRewardLimit = new Dictionary<string, int>();
  }

  /// <summary>
  /// test
  /// </summary>
  /// <exception cref="NullReferenceException">Is null if player has dnt enabled</exception>
  public DatabasePlayer DatabasePlayer { get; private set; } = new DatabasePlayer();
  private void Awake()
  {
    ExiledPlayer = Player.Get(gameObject);
    Logger.Info($"Adding {nameof(AdvancedPlayer)} component to player {ExiledPlayer.Nickname}");

    DayLightDatabase.AddPlayer(ExiledPlayer);
    var dbplayer = DayLightDatabase.GetDatabasePlayer(ExiledPlayer);
    //if (dbplayer == null) return;
    DatabasePlayer = dbplayer;
    if (DatabasePlayer == null) return;
    DatabasePlayer.PropertyChanged += OnPropertyChanged;
    if (!ExiledPlayer.DoNotTrack)
      DatabasePlayer.Stats.PropertyChanged += OnPropertyChanged;
  }
  public void OnPropertyChanged(object sender, PropertyChangedEventArgs ev)
  {
    DayLightDatabase.UpdatePlayer(DatabasePlayer);
  }
  public Player ExiledPlayer { get; private set; }
  
  public string CustomRemoteAdminBadge { get; set; } = "";

  [CanBeNull]
  public CustomRaCategory CustomRaCategory { get; set; } = null;
  
  #region Get
    [CanBeNull]
    public static AdvancedPlayer Get(GameObject gameObject)
    {
      var adv = gameObject.GetComponent<AdvancedPlayer>();
      if(adv == null)
        Logger.Error("AdvancedPlayer is null");
      
      return adv;
      
    }
    [CanBeNull]
    public static AdvancedPlayer Get(int id)
    {
        return !ReferenceHub.TryGetHub(id, out var hub) ? null : hub.gameObject.GetComponent<AdvancedPlayer>();
    }
    [CanBeNull]
    public static AdvancedPlayer Get(ReferenceHub hub) => Get(hub.gameObject);
    [CanBeNull] public static AdvancedPlayer Get(Player apiPlayer) => Get(apiPlayer.ReferenceHub);
    
    [CanBeNull] public static AdvancedPlayer Get(PluginAPI.Core.Player exiledPlayer) => Get(exiledPlayer.ReferenceHub);

    [CanBeNull]
    public static AdvancedPlayer Get(string args)
    {
      try
      {
        if (string.IsNullOrWhiteSpace(args))
          return null;
        if (Player.UserIdsCache.TryGetValue(args, out var player1) && player1.IsConnected)
          return Get(player1);
        if (int.TryParse(args, out var result))
          return Get(Player.Get(result));
        if (args.EndsWith("@steam") || args.EndsWith("@discord") || args.EndsWith("@northwood"))
        {
          foreach (var player2 in Player.Dictionary.Values.Where(player2 => player2.UserId == args))
          {
            player1 = player2;
            break;
          }
        }
        else
        {
          var num1 = 31;
          var lower = args.ToLower();
          foreach (var player3 in Player.Dictionary.Values)
          {
            if (!player3.IsVerified || player3.Nickname == null || !player3.Nickname.Contains(args)) continue;
               var num2 = player3.Nickname.Length - lower.Length;
            if (num2 >= num1) continue;
            num1 = num2;
            player1 = player3;
          }
        }
        if (player1 != null)
          Player.UserIdsCache[player1.UserId] = player1;
        return Get(player1);
      }
      catch (Exception ex)
      {
        Logger.Error($"{(object)typeof(Player).FullName}.{(object)nameof(Get)} error: {(object)ex}");
        return null;
      }
    }
    #endregion



  private void Update()
  {

    if (transform.position.y < -3000)
    {
      if (!Warhead.IsDetonated)
      {
        ExiledPlayer.IsGodModeEnabled = true;
        ExiledPlayer.Teleport(ExiledPlayer.Role.Type.GetRandomSpawnLocation().Position);
        Timing.CallDelayed(1f,
          () =>
          {
            ExiledPlayer.IsGodModeEnabled = false;
          });
      }
      else
      {
        ExiledPlayer.Kill(DamageType.Falldown);
      }
      
    }
  }




}
