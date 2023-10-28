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
public class Vampir : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 100;


    public override uint Id { get; set; } = 36;
    public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
    public override int MaxHealth { get; set; } = 100;
    public override string Name { get; set; } = "Vampir";

    public override string Description { get; set; } =
        "Wenn du jemanden tötest heilst du dich!";

    public override string CustomInfo { get; set; } = "Vampir";

    public override bool KeepInventoryOnSpawn { get; set; } = true;

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new LifeSteal(),
    };

    public override List<string> Inventory { get; set; } = new()
    {
    };
}