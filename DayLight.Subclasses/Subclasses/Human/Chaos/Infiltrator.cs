using DayLight.Core.API.Subclasses.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.Chaos;

[Serializable]
[CustomRole(RoleTypeId.ChaosRepressor)]
public class Infiltrator: Subclass
{
    public override int Chance { get; set; } = 30;

    public override int MaxSpawned { get; set; } = 1;
    public override int MaxHealth { get; set; } = 100;
    
    public override uint Id { get; set; } = 56;
    public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRifleman;
    public override string Name { get; set; } = "Infiltrator";

    public override string Description { get; set; } =
        "Du besitzt Radios.";

    public override string CustomInfo { get; set; } = "Infiltrator";

    public override bool KeepInventoryOnSpawn { get; set; } = false;

    public override List<string> Inventory { get; set; } = new()
    {
        $"{ItemType.GunAK}",
        $"{ItemType.KeycardChaosInsurgency}",
        $"{ItemType.ArmorCombat}",
        $"{ItemType.Medkit}",
        $"{ItemType.Radio}",
        $"{ItemType.Radio}",
        $"{ItemType.Radio}",


    };



    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
    };
}