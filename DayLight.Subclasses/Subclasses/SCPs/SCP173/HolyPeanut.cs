using DayLight.Core.Subclasses.Features;
using DayLight.Subclasses.Abilities.Active;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DayLight.Subclasses.Subclasses.SCPs.SCP173;

[Serializable]
[CustomRole(RoleTypeId.Scp173)]
public class HolyPeanut : Subclass
{
    public override int Chance { get; set; } = 40;
    public override int MaxSpawned { get; set; } = 2;


    public override uint Id { get; set; } = 48;
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp173;
    public override int MaxHealth { get; set; } = 3000;
    public override string Name { get; set; } = "Holy Peanut";

    public override string Description { get; set; } =
        "Du kannst mit .special das Licht aus machen";

    public override string CustomInfo { get; set; } = "Holy Peanut";

    public override bool KeepInventoryOnSpawn { get; set; } = false;
    public override Vector3 Scale { get; set; } = new(1, 1f, 1);

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new Surge(),
    };

    public override List<string> Inventory { get; set; } = new();
}