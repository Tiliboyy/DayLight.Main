﻿using DayLight.Core.Subclasses.Features;
using DayLight.Subclasses.Abilities.Passive;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DayLight.Subclasses.Subclasses.SCPs.SCP049_2;

[Serializable]
[CustomRole(RoleTypeId.Scp0492)]
public class KamikazeZombie : Subclass
{
    public override int Chance { get; set; } = 15;
    public override int MaxSpawned { get; set; } = 2;


    public override uint Id { get; set; } = 53;
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
    public override int MaxHealth { get; set; } = 200;
    public override string Name { get; set; } = "Zombie Kamikaze";

    public override string Description { get; set; } =
        "Du explodierst wenn du stirbst!";

    public override string CustomInfo { get; set; } = "Kamikaze";

    public override bool KeepInventoryOnSpawn { get; set; } = true;
    public override Vector3 Scale { get; set; } = new(1, 1f, 1);

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new ExplodeOnDeath(),
    };

    public override List<string> Inventory { get; set; } = new();
}