using Exiled.API.Enums;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Core;


[Serializable]
public struct ItemPrice
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IgnoreFullInventory { get; set; }
    public List<ItemType> ItemTypes { get; set; }
    public bool IsAmmo { get; set; }
    public Dictionary<AmmoType, ushort> AmmoTypes { get; set; }

    public int Price { get; set; }
    public int MaxAmount { get; set; }
}
[Serializable]
public struct Category
{
    public int id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
    public List<RoleTypeId> AllowedRoles { get; set; }
    public List<ItemPrice> Items { get; set; }
}
[Serializable]
public struct Reward
{
    public string Name { get; set; }
    public Dictionary<RoleTypeId, int> Money { get; set; }
    public int MaxPerRound { get; set; }
}

