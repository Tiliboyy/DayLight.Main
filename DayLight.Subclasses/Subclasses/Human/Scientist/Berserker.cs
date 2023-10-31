using DayLight.Core.API.Subclasses.Features;
using DayLight.Subclasses.Abilities.Active;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.Scientist;

[Serializable]
[CustomRole(RoleTypeId.Scientist)]
public class Berserker : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 1;


    public override uint Id { get; set; } = 52;
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
    public override string Name { get; set; } = "Berserker";
    public override string Description { get; set; } = "Mit .special kannst du einen Bloodlust aktivieren!";
    public override string CustomInfo { get; set; } = "Berserker";
    public override bool KeepInventoryOnSpawn { get; set; } = true;

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new Bloodlust(),
    };

    public override List<string> Inventory { get; set; } = new();
}