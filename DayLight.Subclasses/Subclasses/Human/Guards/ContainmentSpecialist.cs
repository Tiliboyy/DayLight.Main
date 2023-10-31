using DayLight.Core.API.Subclasses.Features;
using DayLight.Subclasses.Abilities.Active;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.Guards;

[Serializable]
[CustomRole(RoleTypeId.FacilityGuard)]
public class ContainmentSpecialist : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 1;
    public override uint Id { get; set; } = 57;
    public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
    public override string Name { get; set; } = "Containment Specialist";

    public override string Description { get; set; } =
        "Du kannst mit .special Informationen über ein SCP anzeigen.";

    public override string CustomInfo { get; set; } = "Containment Specialist";
    public override bool KeepInventoryOnSpawn { get; set; } = false;

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        DynamicSpawnPoints = new List<DynamicSpawnPoint>
        {
            new()
            {
                Location = SpawnLocationType.InsideNukeArmory,
                Chance = 100
            }
        }
    };

    public override List<CustomAbility> CustomAbilities { get; set; } = new()
    {
        new SCPScan()
    };

    public override List<string> Inventory { get; set; } = new()
    {
        $"{ItemType.KeycardContainmentEngineer}",
        $"{ItemType.GunE11SR}",
        $"{ItemType.Medkit}",
        $"{ItemType.ArmorCombat}",
        $"{ItemType.Radio}",
        $"{ItemType.GrenadeHE}"
    };

    public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new()
    {
        { AmmoType.Nato556, 50 }
    };
}