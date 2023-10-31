using DayLight.Core.API.Subclasses.Features;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.Chaos;

[Serializable]
[CustomRole(RoleTypeId.ChaosRepressor)]
public class Juggernaut : Subclass
{
    public override int Chance { get; set; } = 30;

    public override int MaxSpawned { get; set; } = 1;
    public override int MaxHealth { get; set; } = 200;
    
    public override uint Id { get; set; } = 54;
    public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRepressor;
    public override string Name { get; set; } = "Juggernaut";

    public override string Description { get; set; } =
        "Du hast mehr HP, bist jedoch langsamer.";

    public override string CustomInfo { get; set; } = "Juggernaut";

    public override bool KeepInventoryOnSpawn { get; set; } = true;
    
    protected override void RoleAdded(Player player)
    {
        player.EnableEffect(EffectType.Disabled);
        base.RoleAdded(player);
    }

    public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
    {
    };
}