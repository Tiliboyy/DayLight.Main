using DayLight.Core.Subclasses.Features;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.MTF;

[Serializable]
[CustomRole(RoleTypeId.NtfPrivate)]
public class WaffenExperte : Subclass
{
    public override int Chance { get; set; } = 25;
    public override int MaxSpawned { get; set; } = 2;


    public override uint Id { get; set; } = 29;
    public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
    public override int MaxHealth { get; set; } = 120;
    public override string Name { get; set; } = "Waffen Experte";

    public override string Description { get; set; } =
        "Du hast mehr Waffen!";

    public override string CustomInfo { get; set; } = "Waffen Experte";

    public override bool KeepInventoryOnSpawn { get; set; } = false;


    public override List<CustomAbility> CustomAbilities { get; set; } = new();

    public override List<string> Inventory { get; set; } = new()
    {
        $"{ItemType.KeycardMTFOperative}",
        $"{ItemType.GunLogicer}",
        $"{ItemType.GunE11SR}",
        $"{ItemType.Medkit}",
        $"{ItemType.GrenadeHE}",
        $"{ItemType.ArmorHeavy}"
    };

    public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
    {
        { AmmoType.Nato762, 50 },
        { AmmoType.Nato556, 30 }
    };
}