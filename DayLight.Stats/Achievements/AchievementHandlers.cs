#region

using DayLight.Core;
using DayLight.Core.API.Events.EventArgs;
using DayLight.Core.API.Events.Handlers;
using Exiled.API.Enums;
using Exiled.CustomRoles.API;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp079;
using Exiled.Events.EventArgs.Scp330;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
using Exiled.Events.Handlers;
using MEC;
using PlayerRoles;
using PluginAPI.Core;
using Respawning;
using System;
using System.Collections.Generic;
using System.Linq;
using Player = Exiled.API.Features.Player;
using PlayerHandler = Exiled.Events.Handlers.Player;
using ServerHandler = Exiled.Events.Handlers.Server;
using WarheadHandler = Exiled.Events.Handlers.Warhead;

#endregion

namespace DayLight.Stat.Achievements;
internal static class GameStateData
{

    public static Dictionary<Player, AchievementHandlers.GameStats> GameStatsMap = new Dictionary<Player, AchievementHandlers.GameStats>();
    
    public static void ClearGameStats()
    {
        GameStatsMap.Clear();
    }
    
    public static AchievementHandlers.GameStats GetGameStats(this Player player)
    {
        if (player == null) return new AchievementHandlers.GameStats();
      
        if (!GameStatsMap.ContainsKey(player))
        {
            GameStatsMap.Add(player, new AchievementHandlers.GameStats());
        }

        return GameStatsMap[player];
    }
}
internal class AchievementHandlers
{
    public static bool RegisteredEvents;
    public static void RegisterEvents()
    {
        if(RegisteredEvents) return;
        PlayerHandler.Dying += OnDied;
        PlayerHandler.Handcuffing += OnCuffed;
        PlayerHandler.UsedItem += OnUsedItem;
        Scp079.GainingLevel += OnGainingLevel;
        ServerHandler.RespawningTeam += OnRespawningTeam;
        ServerHandler.WaitingForPlayers += OnWaitingForPlayers;
        WarheadHandler.Stopping += OnStoppingWarhead;
        Scp330.EatenScp330 += OnEatenCandy;
        PlayerHandler.ItemAdded += OnAddedItem;
        GameStoreHandler.BuyingItems += SpendingMoney;
        RegisteredEvents = true;
    }

    public static void UnRegisterEvents()
    {
        if(!RegisteredEvents) return;
        PlayerHandler.Dying -= OnDied;
        PlayerHandler.Handcuffing -= OnCuffed;
        PlayerHandler.UsedItem -= OnUsedItem;
        Scp079.GainingLevel -= OnGainingLevel;
        ServerHandler.RespawningTeam -= OnRespawningTeam;
        ServerHandler.WaitingForPlayers -= OnWaitingForPlayers;
        WarheadHandler.Stopping -= OnStoppingWarhead;
        Scp330.EatenScp330 -= OnEatenCandy;
        PlayerHandler.ItemAdded -= OnAddedItem;
        GameStoreHandler.BuyingItems -= SpendingMoney;
        RegisteredEvents = false;
    }

    public static void OnAddedItem(ItemAddedEventArgs ev)
    {
        var data = ev.Player.GetGameStats();
        if (ev.Item.Type == ItemType.GunCom45)
            ev.Player.Achive(19);
        //20Items
        if (!data.LifeCollectedItems.Contains(ev.Item.Type))
            data.LifeCollectedItems.Add(ev.Item.Type);

        if (data.LifeCollectedItems.Count >= 20)
            ev.Player.Achive(21);
        
    }
    public static void SpendingMoney(BuyingItemsEventArgs ev)
    {
        //10 Items
        var data = ev.Player.GetGameStats();
        data.LifeBoughtItems++;
        if (data.LifeBoughtItems >= 10)
            ev.Player.Achive(27);
        //EALight Gayming

        data.SpentGamestoreMoney += ev.Price;
        if (data.SpentGamestoreMoney >= 50000)
            ev.Player.Achive(10);

    }
    public static void OnWaitingForPlayers()
    {
        Timing.RunCoroutine(GameChecks());
        Leaderboard.UpdateLeaderboards();

        RegisterEvents();
    }

    public static void OnCuffed(HandcuffingEventArgs ev)
    {
        if (ev.Target.Role.Team == Team.FoundationForces && ev.Player.Role.Type == RoleTypeId.ClassD)
            ev.Target.Achive(14);
    }

    public static void OnStoppingWarhead(StoppingEventArgs ev)
    {
        var data = ev.Player.GetGameStats();

        data.CancellingNuke++;
        if (data.CancellingNuke >= 3)
            ev.Player.Achive(13);
    }

    public static void OnGainingLevel(GainingLevelEventArgs ev)
    {
        //lvl 5
        if (ev.NewLevel == 5 && Round.Duration.TotalMinutes <= 10)
            ev.Player.Achive(22);
    }

    public static void OnDied(DyingEventArgs ev)
    {
        var data = ev.Player.GetGameStats();

        if (ev.Player == null) return;
        if (ev.DamageHandler.Type == DamageType.Crushed)
            ev.Player.Achive(36);
        if (ev.DamageHandler.Type == DamageType.Decontamination)
            ev.Player.Achive(33);
        //30 Seconds Death
        if (Round.Duration.TotalSeconds <= 30)
            ev.Player.Achive(4);

        ClearLifeStats(data);

        data.Deaths++;


        //5 Deaths
        if (data.Deaths >= 5)
            ev.Player.Achive(8);


        if (ev.Attacker == null) return;
        var attackerdata = ev.Attacker.GetGameStats();

        attackerdata.LifeKills++;
        attackerdata.Kills++;
        if (ev.Player.Role.Type == RoleTypeId.Scp0492)
        {
            attackerdata.LifeZombieKills++;
            if (attackerdata.LifeKills >= 5)
                ev.Attacker.Achive(31);
        }

        //15 Seconds Kills
        if (!attackerdata.LifeTimerKills.ContainsKey(ev.Player))
            attackerdata.LifeTimerKills.Add(ev.Player,
                Round.Duration.TotalSeconds);

        if (ev.Player.RemoteAdminAccess)
            ev.Attacker.Achive(35);
        //Scientist 5 ClassD Kills
        if (ev.Player != null &&
            ev.Attacker.Role.Type == RoleTypeId.Scientist && ev.Player.Role.Type == RoleTypeId.ClassD)
        {
            attackerdata.ClassDKills++;

            if (attackerdata.ClassDKills >= 5)
                ev.Attacker.Achive(17);
        }
        //Kill SCP Paritice
        if (ev.Player.Role.Team == Team.SCPs && ev.Player.Role.Type != RoleTypeId.Scp0492 && ev.DamageHandler.Type == DamageType.ParticleDisruptor)
            ev.Attacker.Achive(34);
        //Kill Scientist with Micro
        if (ev.DamageHandler.Type == DamageType.MicroHid && ev.Attacker.Role.Type == RoleTypeId.ClassD)
            ev.Attacker.Achive(18);
        //Kill Scp as Guard
        if (ev.Player.Role.Team == Team.SCPs && ev.Player.Role.Type != RoleTypeId.Scp0492 &&
            ev.Attacker.Role.Type == RoleTypeId.FacilityGuard)
            ev.Attacker.Achive(6);
        //12 Kills as Human
        if (attackerdata.LifeKills >= 12 &&
            ev.Attacker.Role.Team != Team.SCPs)
            ev.Attacker.Achive(11);
    }
    private static void ClearLifeStats(GameStats data)
    {
        data.LifeChaosSpawnWaves = 0;
        data.LifeMTFSpawnWaves = 0;
        data.LifeKills = 0;
        data.LifeBoughtItems = 0;
        data.LifeZombieKills = 0;

        data.LifeUsedItems.Clear();
        data.LifeCollectedItems.Clear();
        data.LifeTimerKills.Clear();
    }
    public static void OnEatenCandy(EatenScp330EventArgs ev)
    {
        //Diabetis
        var data = ev.Player.GetGameStats();
        
        data.CandyUsed++;
        if (data.CandyUsed >= 8)
            ev.Player.Achive(1);
    }


    public static void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        foreach (var player in Player.List.Where(x => x.IsAlive))
        {
            var data = player.GetGameStats();
            if (ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency)
                data.LifeChaosSpawnWaves++;
            else
                data.LifeMTFSpawnWaves++;
            //2 spawn waves as chaos
            if (player.Role.Team == Team.ChaosInsurgency &&
                data.LifeMTFSpawnWaves >= 2)
                player.Achive(12);
        }
    }

    
    public static void OnUsedItem(UsedItemEventArgs ev)
    {
        if (ev.Item.Type == ItemType.SCP207)
        {
            if (ev.Player.GetEffect(EffectType.AntiScp207).Intensity >= 1)
            {
                ev.Player.Achive(26);

            }
            if (ev.Player.GetEffect(EffectType.Scp207).Intensity + 1 >= 4)
            {
                ev.Player.Achive(25);
            }
        }
        if (ev.Item.Type == ItemType.AntiSCP207)
        {
            if (ev.Player.GetEffect(EffectType.Scp207).Intensity >= 1)
            {
                ev.Player.Achive(26);
            }
        }
        
        var data = ev.Player.GetGameStats();

        if (data.UsedItems.ContainsKey(ev.Item.Type))
        {
            data.UsedItems[ev.Item.Type]++;
        }
        else
        {
            data.UsedItems ??=
                new Dictionary<ItemType, float>();
            data.UsedItems.Add(ev.Item.Type, 1);
        }

        if (data.LifeUsedItems.ContainsKey(ev.Item.Type))
        {
            data.LifeUsedItems[ev.Item.Type]++;

        }
        else
        {
            data.LifeUsedItems.Add(ev.Item.Type, 1);
        }
        if (data.LifeUsedItems.ContainsKey(ItemType.Painkillers) && data.LifeUsedItems[ItemType.Painkillers] >= 20)
            ev.Player.Achive(20);
    }


    public static IEnumerator<float> GameChecks()
    {
        for (;;)
        {
            try
            {
                foreach (var player in Player.List)
                {
                    if (player == null) continue;
                    GameChecks(player);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                throw;
            }

            yield return Timing.WaitForSeconds(1f);
        }
    }

    private static void GameChecks(Player player)
    {
        try
        {
            if(player.Role.Type == RoleTypeId.Tutorial) return;
            if (!player.IsAlive || player.Role.Type == RoleTypeId.Tutorial) return;
            var data = player.GetGameStats();
            var itemsToRemove = (from pair in data.LifeTimerKills where Round.Duration.TotalSeconds - pair.Value > 15 select pair.Key).ToList();

            foreach (var item in itemsToRemove.Where(item => data.LifeTimerKills.ContainsKey(item)))
            {
                data.LifeTimerKills.Remove(item);
            }


            if (player.Role.Type == RoleTypeId.Scp049)
                if (Player.List.Count(x => x.Role.Type == RoleTypeId.Scp0492) >= 10)
                    player.Achive(24);
            if (data.LifeTimerKills.Count >= 7)
                player.Achive(23);
            //10 Spectators
            if (player.CurrentSpectatingPlayers.Count() >= 10)
                player.Achive(5);
            //20 Minutes as Guard
            if (player.Role.Type == RoleTypeId.FacilityGuard && Round.Duration.TotalMinutes >= 20)
                player.Achive(3);
            //Last Stand
            if (Player.List.Count(x => x.IsAlive && !x.IsScp) == 1 && Player.List.Count(x => x.IsScp) >= 1 &&
                Player.List.Count() >= 10 && player.Role.Team != Team.SCPs)
                player.Achive(16);
            //Cola and Piss
            if (player.GetEffect(EffectType.Scp207).Intensity >= 1 &&
                player.GetEffect(EffectType.Scp1853).Intensity >= 1)
                player.Achive(9);

            //Survive
            if (RoundSummary.singleton._roundEnded &&
                player.Role.Type is RoleTypeId.ClassD or RoleTypeId.Scientist or RoleTypeId.FacilityGuard)
                player.Achive(15);

            //Brick
            if (player.GetCustomRoles()?.FirstOrDefault()?.CustomInfo == "Brick")
            {
                player.Achive(28);
            }
            if (player.GetEffect(EffectType.Corroding).Intensity >= 1 && player.GetEffect(EffectType.CardiacArrest).Intensity >= 1)
                player.Achive(29);
        }
        catch (Exception e)
        {
            Log.Error(e.ToString());
        }
    }
    [Serializable]
    public class GameStats
    {
        public int Kills { get; set; }
        
        public int CandyUsed { get; set; }
        public int Deaths { get; set; }

        public int LifeZombieKills { get; set; }

        public int ClassDKills { get; set; }

        public int CancellingNuke { get; set; }
        public float SpentGamestoreMoney { get; set; }
        public int LifeBoughtItems { get; set; }
        public int LifeKills { get; set; }

        public int LifeMTFSpawnWaves { get; set; }
        public int LifeChaosSpawnWaves { get; set; }
        
        public Dictionary<Player, double> LifeTimerKills { get; set; } = new();
        
        public Dictionary<ItemType, float> UsedItems { get; set; } = new();

        public Dictionary<ItemType, float> LifeUsedItems { get; set; } = new();
        public List<ItemType> LifeCollectedItems { get; set; } = new();
    }

}