using DayLight.Core.API.Subclasses.Features;
using DayLight.Subclasses.Abilities.Passive;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.Chaos;

[Serializable]
[CustomRole(RoleTypeId.ChaosRifleman)]
public class Kamikaze : Subclass
{
    public override int Chance { get; set; } = 30;

    public override int MaxSpawned { get; set; } = 1;

    public override uint Id { get; set; } = 11;
    public override RoleTypeId Role { get; set; } = RoleTypeId.ChaosRifleman;
    public override string Name { get; set; } = "Kamikaze";

    public override string Description { get; set; } =
        "Du nimmst keinen schaden vom SCP-207!";

    public override string CustomInfo { get; set; } = "Kamikaze";

    public override bool KeepInventoryOnSpawn { get; set; } = false;

    public override List<string> Inventory { get; set; } = new()
    {
        $"{ItemType.Medkit}",
        $"{ItemType.GunAK}",
        $"{ItemType.KeycardChaosInsurgency}",
        $"{ItemType.SCP207}",
        $"{ItemType.SCP207}",
        $"{ItemType.SCP207}",
        $"{ItemType.SCP207}"
    };

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new No207Damage(),
    };
}