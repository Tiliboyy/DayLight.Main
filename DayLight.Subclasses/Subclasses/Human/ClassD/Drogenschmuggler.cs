using DayLight.Core.API.Subclasses.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.ClassD;

[Serializable]
[CustomRole(RoleTypeId.ClassD)]
public class Drogenschmuggler : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 100;


    public override uint Id { get; set; } = 22;
    public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
    public override string Name { get; set; } = "Drogenschmuggler";
    public override string Description { get; set; } = "Du hast Drogen!";
    public override string CustomInfo { get; set; } = "Drogenschmuggler";
    public override bool KeepInventoryOnSpawn { get; set; } = false;
    public override List<CustomAbility> CustomAbilities { get; set; } = new();

    public override List<string> Inventory { get; set; } = new()
    {
        $"{ItemType.Painkillers}",
        $"{ItemType.Painkillers}",
        $"{ItemType.Painkillers}",
        $"{ItemType.Painkillers}",
        $"{ItemType.Painkillers}",
        $"{ItemType.Painkillers}",
        $"{ItemType.Painkillers}",
        $"{ItemType.Painkillers}"
    };
}