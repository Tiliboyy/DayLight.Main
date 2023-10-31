using DayLight.Core.API.Subclasses.Features;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DayLight.Core.API.Subclasses;

public class Manager
{
    public static List<Subclass> Subclasses = new();
    public static List<Player> NoRoleAllowed = new();
    public static bool NoRandomRole = false;

    public class Subclass
    {
        public Subclass(string className, RoleTypeId classRoleType, int classChance, int maxSpawnAmount,
            int spawnedAmount)
        {
            ClassName = className;
            ClassRoleType = classRoleType;
            ClassChance = classChance;
            MaxSpawnAmount = maxSpawnAmount;
            SpawnedAmount = spawnedAmount;
        }

        public string ClassName { get; set; }
        public RoleTypeId ClassRoleType { get; set; }
        public int ClassChance { get; set; }

        public int MaxSpawnAmount { get; set; }

        public int SpawnedAmount { get; set; }
    }
}

public class Utils
{
    public static string GetSCPRoleName(Player player) => player.Role.Type switch
    {
        RoleTypeId.Scp939 => "SCP-939",
        RoleTypeId.Scp173 => "SCP-173",
        RoleTypeId.Scp106 => "SCP-106",
        RoleTypeId.Scp049 => "SCP-049",
        RoleTypeId.Scp0492 => "SCP-049-2",
        RoleTypeId.Scp096 => "SCP-096",
        RoleTypeId.Scp079 => "SCP-079",
        _ => string.Empty
    };
    public static string GetZoneName(ZoneType zone)
    {
        return zone switch
        {
            ZoneType.Unspecified => "Unspecified",
            ZoneType.LightContainment => "LCZ",
            ZoneType.HeavyContainment => "HCZ",
            ZoneType.Entrance => "Entrance",
            ZoneType.Surface => "Surface",
            ZoneType.Other => "Unknown",
            _ => "Invalid zone type."
        };
    }
    public static string GetHPColor(Player player)
    {
        float hp = player.Health / player.MaxHealth;
        return hp switch
        {
            > 0.7f => "green",
            <= 0.7f and >= 0.4f => "yellow",
            _ => "red"
        };
    }
    public static IEnumerator<float> TryAddInventory(Player player, List<string> Items, Subclass role)
    {
        var originalrole = player.Role.Type;
        foreach (var item in Items.TakeWhile(item => player.Role.Type == originalrole))
        {
            TryAddItem(player, item, role);
            yield return Timing.WaitForSeconds(0.1f);
        }
    }


    public static void TryAddItem(Player player, string itemName, Subclass role)
    {
        if (CustomItem.TryGet(itemName, out var customItem))
        {
            customItem?.Give(player, role.DisplayCustomItemMessages);
            return;
        }

        if (Enum.TryParse(itemName, out ItemType result))
        {
            if (result.IsAmmo())
                player.Ammo[result] = 100;
            else
                player.AddItem(result);
            return;
        }

        Logger.Warn(role.Name + ": TryAddItem: " + itemName + " is not a valid ItemType or Custom Item name.");
    }
}