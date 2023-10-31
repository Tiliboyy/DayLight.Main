using DayLight.Core.API.Subclasses.Features;
using DayLight.Subclasses.Abilities.Passive;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.ClassD;

[Serializable]
[CustomRole(RoleTypeId.ClassD)]
public class StarkeClassD : Subclass
{
    public override int Chance { get; set; } = 10;

    public override uint Id { get; set; } = 25;
    public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
    public override string Name { get; set; } = "Starke D-Klasse";

    public override string Description { get; set; } =
        "Du kannst Gates aufreißen!";

    public override string CustomInfo { get; set; } = "Starke D-Klasse";
    public override bool KeepInventoryOnSpawn { get; set; } = true;

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new PryGate()
    };

    public override List<string> Inventory { get; set; } = new();
}
