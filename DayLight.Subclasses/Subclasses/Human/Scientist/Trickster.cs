using DayLight.Core.Subclasses.Features;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.Scientist;

[Serializable]
[CustomRole(RoleTypeId.Scientist)]
public class Trickster : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 1;


    public override uint Id { get; set; } = 32;
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
    public override string Name { get; set; } = "Trickster";

    public override string Description { get; set; } =
        "Du besitzt eine Waffe und SCP-268!";

    public override string CustomInfo { get; set; } = "Trickster";

    public override bool KeepInventoryOnSpawn { get; set; } = true;


    public override List<CustomAbility> CustomAbilities { get; set; } = new();

    public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new()
    {
        { AmmoType.Nato9, 30 }
    };

    public override List<string> Inventory { get; set; } = new()
    {
        $"{ItemType.SCP268}",
        $"{ItemType.GunCOM18}"
    };
}