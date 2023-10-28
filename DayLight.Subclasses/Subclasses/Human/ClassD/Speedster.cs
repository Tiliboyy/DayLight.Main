using DayLight.Core.Subclasses.Features;
using DayLight.Subclasses.Abilities.Passive;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.ClassD;

[Serializable]
[CustomRole(RoleTypeId.ClassD)]
public class Speedster : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 100;


    public override uint Id { get; set; } = 24;
    public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
    public override string Name { get; set; } = "Speedster";

    public override string Description { get; set; } =
        "Für jeden Kill, den du machst, wirst du 5% schneller!";

    public override string CustomInfo { get; set; } = "Speedster";

    public override bool KeepInventoryOnSpawn { get; set; } = false;

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new SpeedOnKill(),
    };

    public override List<string> Inventory { get; set; } = new()
    {
        $"{ItemType.SCP207}",
        $"{ItemType.Adrenaline}",
        $"{ItemType.Medkit}"
    };
}