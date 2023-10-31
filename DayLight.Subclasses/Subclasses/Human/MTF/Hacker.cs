using DayLight.Core.API.Subclasses.Features;
using DayLight.Subclasses.Abilities.Passive;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.MTF;

[Serializable]
[CustomRole(RoleTypeId.NtfPrivate)]
public class Hacker : Subclass
{
    public override int Chance { get; set; } = 20;
    public override int MaxSpawned { get; set; } = 3;


    public override uint Id { get; set; } = 28;
    public override RoleTypeId Role { get; set; } = RoleTypeId.NtfPrivate;
    public override string Name { get; set; } = "Hacker";

    public override string Description { get; set; } =
        $"Du kannst mit deine Ability eine belibige tür öffnen";

    public override string CustomInfo { get; set; } = "Hacker";

    public override bool KeepInventoryOnSpawn { get; set; } = true;

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new Hack()
    };

    public override List<string> Inventory { get; set; } = new();
}