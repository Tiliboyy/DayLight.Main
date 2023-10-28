/*using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using MEC;
using PlayerRoles;
using UnityEngine;
using CustomRole = Subclasses.Features.CustomRole;

namespace Subclasses.Subclasses.SCPs.SCP049;

[Serializable]
[CustomRole(RoleTypeId.Scp049)]
public class SpeedyDoctor : CustomRole
{
    public override int Chance { get; set; } = 40;
    public override int MaxSpawned { get; set; } = 2;


    public override uint Id { get; set; } = 56;
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scp049;
    public override string Name { get; set; } = "Speedy Doctor";

    public override string Description { get; set; } =
        "Du bist schneller!";

    public override string CustomInfo { get; set; } = "Speedy Doctor";

    public override bool KeepInventoryOnSpawn { get; set; } = true;
    public override Vector3 Scale { get; set; } = new(1, 1f, 1);

    public override List<CustomAbility> CustomAbilities { get; set; } = new();

    public override List<string> Inventory { get; set; } = new();

    protected override void RoleAdded(Player player)
    {
        player.ChangeEffectIntensity<MovementBoost>(10, 0);
        base.RoleAdded(player);
    }
}*/