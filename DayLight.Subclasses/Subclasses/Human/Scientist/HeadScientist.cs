using DayLight.Core.API.Subclasses.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.Scientist;

[Serializable]
[CustomRole(RoleTypeId.Scientist)]
public class HeadScientist : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 2;


    public override uint Id { get; set; } = 31;
    public override RoleTypeId Role { get; set; } = RoleTypeId.Scientist;
    public override string Name { get; set; } = "Hauptwissenschaftler";

    public override string Description { get; set; } =
        "Du besitzt eine bessere Karte!";

    public override string CustomInfo { get; set; } = "Hauptwissenschaftler";

    public override bool KeepInventoryOnSpawn { get; set; } = false;


    public override List<CustomAbility> CustomAbilities { get; set; } = new();

    public override List<string> Inventory { get; set; } = new()
    {
        $"{ItemType.Medkit}",
        $"{ItemType.KeycardResearchCoordinator}",
        $"{ItemType.Painkillers}"
    };
}