using DayLight.Core.API.Subclasses.Features;
using DayLight.Subclasses.Abilities.Passive;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.Scientist;

[Serializable]
[CustomRole(RoleTypeId.Scientist)]
public class GuterLäufer : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 1;


    public override uint Id { get; set; } = 30;
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
    public override string Name { get; set; } = "Rubuster Läufer";

    public override string Description { get; set; } =
        "Du wirst von Sinkholes oder SCP-173 Tantrums nicht verlangsamt!";

    public override string CustomInfo { get; set; } = "Rubuster Wanderer";

    public override bool KeepInventoryOnSpawn { get; set; } = true;


    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new NoHazards()
    };

    public override List<string> Inventory { get; set; } = new();
}