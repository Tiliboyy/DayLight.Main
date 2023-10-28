using DayLight.Core.Subclasses.Features;
using DayLight.Subclasses.Abilities.Passive;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.MTF;

[Serializable]
[CustomRole(RoleTypeId.NtfSergeant)]
public class Sniper : Subclass
{
    public override int Chance { get; set; } = 30;
    public override int MaxSpawned { get; set; } = 1;


    public override uint Id { get; set; } = 55;
    public override RoleTypeId Role { get; set; } = RoleTypeId.NtfSergeant;
    public override int MaxHealth { get; set; } = 100;
    public override string Name { get; set; } = "Sniper";

    public override string Description { get; set; } =
        "Du hast eine Sniper!";

    public override string CustomInfo { get; set; } = "Sniper";

    public override bool KeepInventoryOnSpawn { get; set; } = false;


    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new SeeHP(),
    };

    public override List<string> Inventory { get; set; } = new()
    {
        $"{ItemType.KeycardMTFOperative}",
        $"Sniper",
        $"{ItemType.GunCOM18}",
        $"{ItemType.Medkit}",
        $"{ItemType.ArmorCombat}"
    };

    public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>()
    {
        { AmmoType.Nato762, 50 },
        { AmmoType.Nato9, 30 }
    };
}