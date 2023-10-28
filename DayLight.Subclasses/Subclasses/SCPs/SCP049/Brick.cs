﻿using DayLight.Core.Subclasses.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DayLight.Subclasses.Subclasses.SCPs.SCP049;

[Serializable]
[CustomRole(RoleTypeId.Scp049)]
public class DoctorBrick : Subclass
{
    public override int Chance { get; set; } = 1;
    public override int MaxSpawned { get; set; } = 2;


    public override uint Id { get; set; } = 41;
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp049;
    public override string Name { get; set; } = "049 Brick";

    public override string Description { get; set; } =
        "Du bist flach";

    public override string CustomInfo { get; set; } = "Brick";

    public override bool KeepInventoryOnSpawn { get; set; } = true;
    public override Vector3 Scale { get; set; } = new(1, 0.5f, 1);

    public override List<CustomAbility> CustomAbilities { get; set; } = new();

    public override List<string> Inventory { get; set; } = new();
}