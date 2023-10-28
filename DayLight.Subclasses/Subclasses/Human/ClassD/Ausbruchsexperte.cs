using DayLight.Core.Subclasses.Features;
using DayLight.Subclasses.Abilities.Active;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DayLight.Subclasses.Subclasses.Human.ClassD;

[Serializable]
[CustomRole(RoleTypeId.ClassD)]
public class Ausbruchsexperte : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 1;


    public override uint Id { get; set; } = 35;
    public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
    public override string Name { get; set; } = "Ausbruchsexperte";

    public override string Description { get; set; } =
        "Du bist Ausbruchsexperte\nDu kannst mit .special deinen Tot vortäuschen.";

    public override string CustomInfo { get; set; } = "Ausbruchsexperte";

    public override bool KeepInventoryOnSpawn { get; set; } = true;
    public override Vector3 Scale { get; set; } = new(1, 1f, 1);

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new FakeDeath(),
    };

    public override List<string> Inventory { get; set; } = new()
    {
        $"{ItemType.Adrenaline}",
    };
}