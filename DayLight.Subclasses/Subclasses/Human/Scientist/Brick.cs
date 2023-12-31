﻿using DayLight.Core.API.Subclasses.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DayLight.Subclasses.Subclasses.Human.Scientist;

[Serializable]
[CustomRole(RoleTypeId.Scientist)]
public class ScientistBrick : Subclass
{
    public override int Chance { get; set; } = 1;
    public override int MaxSpawned { get; set; } = 1;


    public override uint Id { get; set; } = 33;
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
    public override string Name { get; set; } = "Wissenschaftler Brick";

    public override string Description { get; set; } =
        "Du bist flach";

    public override string CustomInfo { get; set; } = "Brick";

    public override bool KeepInventoryOnSpawn { get; set; } = true;
    public override Vector3 Scale { get; set; } = new(1, 0.5f, 1);

    public override List<CustomAbility> CustomAbilities { get; set; } = new();

    public override List<string> Inventory { get; set; } = new();
}