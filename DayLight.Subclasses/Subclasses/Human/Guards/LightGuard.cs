using DayLight.Core.Subclasses.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.Guards;

[Serializable]
[CustomRole(RoleTypeId.FacilityGuard)]
public class LightGuard : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 1;
    public override uint Id { get; set; } = 26;
    public override RoleTypeId Role { get; set; } = RoleTypeId.FacilityGuard;
    public override string Name { get; set; } = "Light Guard";
    public override string Description { get; set; } = "Du spawnst in der LCZ!";
    public override string CustomInfo { get; set; } = "Light Guard";
    public override bool KeepInventoryOnSpawn { get; set; } = false;

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        RoleSpawnPoints = new List<RoleSpawnPoint>
        {
            new()
            {
                Role = RoleTypeId.Scientist,
                Chance = 100
            }
        }
    };

    public override List<CustomAbility> CustomAbilities { get; set; } = new();

    public override List<string> Inventory { get; set; } = new()
    {
        $"{ItemType.KeycardGuard}",
        $"{ItemType.GunCOM18}",
        $"{ItemType.Medkit}",
        $"{ItemType.ArmorLight}"
    };
}