using DayLight.Core.API.Subclasses.Features;
using DayLight.Subclasses.Abilities.Passive;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.Chaos;

[Serializable]
[CustomRole(RoleTypeId.ChaosRepressor)]
public class Executor: Subclass
{
    public override int Chance { get; set; } = 30;

    public override int MaxSpawned { get; set; } = 1;
    public override int MaxHealth { get; set; } = 100;
    
    public override uint Id { get; set; } = 55;
    public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRepressor;
    public override string Name { get; set; } = "Executor";

    public override string Description { get; set; } =
        "Du wurdest als schnelles Exekutionsteam geschickt.";

    public override string CustomInfo { get; set; } = "Executor";

    public override bool KeepInventoryOnSpawn { get; set; } = false;

    public override List<string> Inventory { get; set; } = new List<string>()
    {
        $"{ItemType.GunRevolver}",
        $"{ItemType.KeycardChaosInsurgency}",
        $"{ItemType.ArmorCombat}",
        $"{ItemType.Medkit}",
        $"{ItemType.SCP207}",
        $"{ItemType.SCP207}",


    };
    
    public override List<CustomAbility> CustomAbilities { get; set; } = new List<CustomAbility>()
    {
        new No207Damage(),
    };
}