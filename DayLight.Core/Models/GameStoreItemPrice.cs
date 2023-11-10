using Exiled.API.Enums;
using System;
using System.Collections.Generic;

namespace DayLight.Core.Models;

[Serializable]
public struct GameStoreItemPrice
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
