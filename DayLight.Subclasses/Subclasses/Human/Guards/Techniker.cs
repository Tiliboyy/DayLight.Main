using DayLight.Core.API.Subclasses.Features;
using DayLight.Subclasses.Abilities.Active;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.Guards;

[Serializable]
[CustomRole(RoleTypeId.FacilityGuard)]
public class Techniker : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 2;

    public override uint Id { get; set; } = 27;
    public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
    public override string Name { get; set; } = "Techniker";
    public override string Description { get; set; } = "Du kannst mit .special Venten";
    public override string CustomInfo { get; set; } = "Techniker";
    public override bool KeepInventoryOnSpawn { get; set; } = true;

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new Vent()
    };
}