using CustomPlayerEffects;
using DayLight.Core.Subclasses.Features;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DayLight.Subclasses.Subclasses.SCPs.SCP049_2;

[Serializable]
[CustomRole(RoleTypeId.Scp0492)]
public class HungrigerZombie : Subclass
{
    public override int Chance { get; set; } = 15;
    public override int MaxSpawned { get; set; } = 51;


    public override uint Id { get; set; } = 49;
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp0492;
    public override int MaxHealth { get; set; } = 200;
    public override string Name { get; set; } = "Hungriger Zombie";

    public override string Description { get; set; } =
        "Du bist schneller hast aber weniger HP!";

    public override string CustomInfo { get; set; } = "Hungriger Zombie";

    public override bool KeepInventoryOnSpawn { get; set; } = true;
    public override Vector3 Scale { get; set; } = new(1, 1f, 1);

    protected override void RoleAdded(Player player)
    {
        player.ChangeEffectIntensity<MovementBoost>(30, 0f);
        base.RoleAdded(player);
    }

    public override List<string> Inventory { get; set; } = new();
}