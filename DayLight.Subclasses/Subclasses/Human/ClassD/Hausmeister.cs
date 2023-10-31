using DayLight.Core.API.Subclasses.Features;
using Exiled.API.Enums;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Subclasses.Subclasses.Human.ClassD;

[Serializable]
[CustomRole(RoleTypeId.ClassD)]
public class Hausmeister : Subclass
{
    public override int Chance { get; set; } = 10;
    public override int MaxSpawned { get; set; } = 100;

    public override SpawnProperties SpawnProperties { get; set; } = new()
    {
        DynamicSpawnPoints = new List<DynamicSpawnPoint>
        {
            new()
            {
                Location = SpawnLocationType.Inside914,
                Chance = 100
            }
        }
    };

    public override uint Id { get; set; } = 23;
    public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;
    public override string Name { get; set; } = "Hausmeister";
    public override string Description { get; set; } = "Du besitzt eine Hausmeister Karte und spawnst bei SCP-914.";
    public override string CustomInfo { get; set; } = "Hausmeister";
    public override bool KeepInventoryOnSpawn { get; set; } = true;
    public override List<CustomAbility> CustomAbilities { get; set; } = new();

    public override List<string> Inventory { get; set; } = new()
    {
        $"{ItemType.KeycardJanitor}"
    };
}