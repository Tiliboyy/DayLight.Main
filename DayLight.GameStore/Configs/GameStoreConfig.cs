using DayLight.Core.Models;
using Exiled.API.Enums;
using Neuron.Core.Meta;
using PlayerRoles;
using Syml;
using System.Collections.Generic;
using System.ComponentModel;
using IConfig = DayLight.Core.IConfig;

namespace DayLight.GameStore.Configs;

[Automatic]
[DocumentSection("DayLight.GameStore")]
public class GameStoreConfig : IConfig
{
    [Description("Enables the Plugin")] 
    public bool Enabled { get; set; }
    public bool Debug { get; set; } = false;
    public bool ShowOnlyAvalibleItems { get; set; }= true;

    public bool EnableLimit { get; set; } = false;
    
    public float MoneyLimit { get; set; } = 200000;
    
    [Description("The amount a player gets from each event. 0 disables the event. -1 Is unlimited")]
    public GameStoreReward EscapeGameStoreReward { get; set; } = new()
    {
        Name = "Escape",

        Money = new Dictionary<RoleTypeId, int>
        {
            { RoleTypeId.Scientist, 2000 },
            { RoleTypeId.ClassD, 2500 }
        },
        MaxPerRound = 1
    };
    [Description("The amount of money the cuffer of an escaped player gets when he escapes")]
    public GameStoreReward CufferGameStoreReward { get; set; } = new()
    {
        Name = "EscapeCuffer",
        Money = new Dictionary<RoleTypeId, int>
        {
            { RoleTypeId.None, 1000 }
        },
        MaxPerRound = 1
    };
    [Description("The amount of money SCP079 gets when leveling")]

    public GameStoreReward Scp079LevelGameStoreReward { get; set; } = new()
    {
        Name = "Scp079Level",

        Money = new Dictionary<RoleTypeId, int>
        {
            { RoleTypeId.Scp079, 500 },
        },
        MaxPerRound = -1
    };
    [Description("The amount of money you get per kill as that role")]
    public GameStoreReward KillGameStoreReward { get; set; } = new()
    {
        Name = "Kill",
        Money = new Dictionary<RoleTypeId, int>
        {
            { RoleTypeId.ChaosConscript, 50 },
            { RoleTypeId.ChaosMarauder, 50 },
            { RoleTypeId.ChaosRepressor, 50 },
            { RoleTypeId.ChaosRifleman, 50 },
            { RoleTypeId.ClassD, 50 },
            { RoleTypeId.FacilityGuard, 50 },
            { RoleTypeId.NtfCaptain, 50 },
            { RoleTypeId.NtfPrivate, 50 },
            { RoleTypeId.NtfSergeant, 50 },
            { RoleTypeId.NtfSpecialist, 50 },
            { RoleTypeId.Scientist, 50 },
            { RoleTypeId.Scp049, 100 },
            { RoleTypeId.Scp079, 100 },
            { RoleTypeId.Scp096, 100 },
            { RoleTypeId.Scp106, 100 },
            { RoleTypeId.Scp0492, 100 },
            { RoleTypeId.Scp939, 100 },
            { RoleTypeId.Scp173, 50 },
            { RoleTypeId.Tutorial, 0 }
        },
        MaxPerRound = -1
    };
    [Description("The amount of money the killer of an scp gets")]

    public GameStoreReward ScpKillGameStoreReward { get; set; } = new()
    {
        Name = "SCPKilled",

        Money = new Dictionary<RoleTypeId, int>
        {
            { RoleTypeId.None, 1000 }
        },
        MaxPerRound = 1
    };
    [Description("The amount of money a player gets when he dies")]

    public GameStoreReward DeathGameStoreReward { get; set; } = new()
    {
        Name = "Died",

        Money = new Dictionary<RoleTypeId, int>
        {
            { RoleTypeId.None, 50 }
        },
        MaxPerRound = -1
    };
    [Description("The amount of money a player gets when he is using an item")]

    public GameStoreReward UsingItemGameStoreReward { get; set; } = new()
    {
        Name = "UsingItem",

        Money = new Dictionary<RoleTypeId, int>
        {
            { RoleTypeId.None, 50 }
        },
        MaxPerRound = -1
    };
    [Description("The amount of money a player gets when he spawns as a role")]

    public GameStoreReward SpawnGameStoreReward { get; set; } = new()
    {
        Name = "Spawned",

        Money = new Dictionary<RoleTypeId, int>
        {
            { RoleTypeId.ChaosConscript, 50 },
            { RoleTypeId.ChaosMarauder, 50 },
            { RoleTypeId.ChaosRepressor, 50 },
            { RoleTypeId.ChaosRifleman, 50 },
            { RoleTypeId.ClassD, 200 },
            { RoleTypeId.FacilityGuard, 200 },
            { RoleTypeId.NtfCaptain, 50 },
            { RoleTypeId.NtfPrivate, 50 },
            { RoleTypeId.NtfSergeant, 50 },
            { RoleTypeId.NtfSpecialist, 50 },
            { RoleTypeId.Scientist, 200 },
            { RoleTypeId.Scp049, 200 },
            { RoleTypeId.Scp079, 200 },
            { RoleTypeId.Scp096, 200 },
            { RoleTypeId.Scp106, 200 },
            { RoleTypeId.Scp0492, 200 },
            { RoleTypeId.Scp939, 200 },
            { RoleTypeId.Tutorial , 0}
        },
        MaxPerRound = -1
    };

    public List<Category> Categorys { get; set; } = new()
    {
        new Category
        {
            id = 1, AllowedRoles = new List<RoleTypeId> { RoleTypeId.ClassD }, Name = "D-Klasse",
            Description = "Hier kannst du Gegenstände für D-Klassen kaufen.",
            Items = new List<GameStoreItemPrice>
            {
                new()
                {
                    Id = 1,
                    Price = 2000,
                    IgnoreFullInventory = false,
                    Name = "Hausmeisterkarte",
                    ItemTypes = new List<ItemType> { ItemType.KeycardJanitor },
                    MaxAmount = 1
                },

                new()
                {
                    Id = 2,
                    Price = 4000,
                    IgnoreFullInventory = false,
                    Name = "Wissenschaftlerkarte",
                    ItemTypes = new List<ItemType> { ItemType.KeycardScientist },
                    MaxAmount = 1
                },

                new()
                {
                    Id = 3,
                    Price = 600,
                    IgnoreFullInventory = false,
                    Name = "Schmerzmittel",
                    ItemTypes = new List<ItemType> { ItemType.Painkillers },
                    MaxAmount = 2
                }
            }
        },
        new Category
        {
            id = 2,
            Name = "Wissenschaftler",
            AllowedRoles = new List<RoleTypeId> { RoleTypeId.Scientist },
            Description = "Hier kannst du Gegenstände für Wissenschaftler kaufen.",
            Items = new List<GameStoreItemPrice>
            {
                new()
                {
                    Id = 1,
                    Price = 4000,
                    IgnoreFullInventory = false,
                    Name = "Hauptwissenschaftlerkarte",
                    ItemTypes = new List<ItemType> { ItemType.KeycardResearchCoordinator },
                    MaxAmount = 1
                },

                new()
                {
                    Id = 2,
                    Price = 5000,
                    IgnoreFullInventory = false,
                    Name = "Zonenmanagerkarte",
                    ItemTypes = new List<ItemType> { ItemType.KeycardZoneManager },
                    MaxAmount = 1
                },

                new()
                {
                    Id = 3,
                    Price = 500,
                    IgnoreFullInventory = false,
                    Name = "Radio",
                    ItemTypes = new List<ItemType> { ItemType.Radio },
                    MaxAmount = 2
                },
                new()
                {
                    Id = 4,
                    Price = 2000,
                    IgnoreFullInventory = false,
                    Name = "SCP-500",
                    ItemTypes = new List<ItemType> { ItemType.SCP500 },
                    MaxAmount = 2
                },

                new()
                {
                    Id = 5,
                    Price = 400,
                    IgnoreFullInventory = false,
                    Name = "Schmerzmittel",
                    ItemTypes = new List<ItemType> { ItemType.Painkillers },
                    MaxAmount = 2
                },
                new()
                {
                    Id = 6,
                    Price = 5000,
                    IgnoreFullInventory = false,
                    Name = "SCP-018",
                    ItemTypes = new List<ItemType> { ItemType.SCP018 },
                    MaxAmount = 2
                },
                new()
                {
                    Id = 7,
                    Price = 7000,
                    IgnoreFullInventory = false,
                    Name = "SCP-268",
                    ItemTypes = new List<ItemType> { ItemType.SCP268 },
                    MaxAmount = 1
                },
                new()
                {
                    Id = 8,
                    Price = 3500,
                    IgnoreFullInventory = false,
                    Name = "Leichte Brustpanzerung",
                    ItemTypes = new List<ItemType> { ItemType.ArmorLight },
                    MaxAmount = 5
                }
            }
        },
        new Category
        {
            id = 3, AllowedRoles = new List<RoleTypeId> { RoleTypeId.FacilityGuard }, Name = "Sicherheitspersonal",
            Description = "Hier kannst du Gegenstände für Sicherheitspersonal kaufen.",
            Items = new List<GameStoreItemPrice>
            {
                new()
                {
                    Id = 1,
                    Price = 3000,
                    IgnoreFullInventory = false,
                    Name = "Kadettenkarte",
                    ItemTypes = new List<ItemType> { ItemType.KeycardMTFPrivate },
                    MaxAmount = 1
                },


                new()
                {
                    Id = 2,
                    Price = 4000,
                    IgnoreFullInventory = false,
                    Name = "Crossvec",
                    ItemTypes = new List<ItemType> { ItemType.GunCrossvec },
                    MaxAmount = 1
                },


                new()
                {
                    Id = 3,
                    Price = 1000,
                    IgnoreFullInventory = false,
                    Name = "Granate",
                    ItemTypes = new List<ItemType> { ItemType.GrenadeHE },
                    MaxAmount = 2
                },

                new()
                {
                    Id = 4,
                    Price = 1000,
                    IgnoreFullInventory = false,
                    Name = "Flash Grenade",
                    ItemTypes = new List<ItemType> { ItemType.GrenadeFlash },
                    MaxAmount = 2
                },

                new()
                {
                    Id = 5,
                    Price = 4500,
                    IgnoreFullInventory = false,
                    Name = "Schwere Brustpanzerung",
                    ItemTypes = new List<ItemType> { ItemType.ArmorHeavy },
                    MaxAmount = 1
                },


                new()
                {
                    Id = 6,
                    Price = 500,
                    IgnoreFullInventory = false,
                    Name = "Schmerzmittel",
                    ItemTypes = new List<ItemType> { ItemType.Painkillers },
                    MaxAmount = 2
                }
            }
        },
        new Category
        {
            id = 4,
            AllowedRoles = new List<RoleTypeId>
                { RoleTypeId.NtfCaptain, RoleTypeId.NtfPrivate, RoleTypeId.NtfSergeant, RoleTypeId.NtfSpecialist },
            Name = "MTF", Description = "Hier kannst du Gegenstände für das MTF kaufen.",
            Items = new List<GameStoreItemPrice>
            {
                new()
                {
                    Id = 1,
                    Price = 7000,
                    IgnoreFullInventory = false,
                    Name = "Facilitymanager Karte",
                    ItemTypes = new List<ItemType> { ItemType.KeycardFacilityManager },
                    MaxAmount = 1
                },


                new()
                {
                    Id = 2,
                    Price = 2500,
                    Name = "MTF-E11-SR",
                    IgnoreFullInventory = false,
                    ItemTypes = new List<ItemType> { ItemType.GunE11SR },
                    MaxAmount = 1
                },

                new()
                {
                    Id = 3,
                    Price = 3500,
                    IgnoreFullInventory = false,
                    Name = "Schwere Brustpanzerung",
                    ItemTypes = new List<ItemType> { ItemType.ArmorHeavy },
                    MaxAmount = 1
                },

                new()
                {
                    Id = 4,
                    Price = 1500,
                    IgnoreFullInventory = false,
                    Name = "SCP-500",
                    ItemTypes = new List<ItemType> { ItemType.SCP500 },
                    MaxAmount = 2
                },

                new()
                {
                    Id = 5,
                    Price = 500,
                    IgnoreFullInventory = false,
                    Name = "Schmerzmittel",
                    ItemTypes = new List<ItemType> { ItemType.Painkillers },
                    MaxAmount = 2
                },

                new()
                {
                    Id = 6,
                    Price = 1000,
                    IgnoreFullInventory = false,
                    Name = "Granate",
                    ItemTypes = new List<ItemType> { ItemType.GrenadeHE },
                    MaxAmount = 2
                },


                new()
                {
                    Id = 7,
                    Price = 750,
                    IgnoreFullInventory = false,
                    Name = "Flash Granate",
                    ItemTypes = new List<ItemType> { ItemType.GrenadeFlash },
                    MaxAmount = 2
                },

                new()
                {
                    Id = 8,
                    Price = 50000,
                    IgnoreFullInventory = false,
                    Name = "X3-Particle-Disruptor",
                    ItemTypes = new List<ItemType> { ItemType.ParticleDisruptor },
                    MaxAmount = 1
                }
            }
        },
        new Category
        {
            id = 5,
            AllowedRoles = new List<RoleTypeId>
            {
                RoleTypeId.ChaosConscript, RoleTypeId.ChaosMarauder, RoleTypeId.ChaosRepressor, RoleTypeId.ChaosRifleman
            },
            Name = "Chaos", Description = "Hier kannst du Gegenstände für Chaos insurgency kaufen.",
            Items = new List<GameStoreItemPrice>
            {
                new()
                {
                    Id = 1,
                    Price = 7000,
                    IgnoreFullInventory = false,
                    Name = "Facilitymanager Karte",
                    ItemTypes = new List<ItemType> { ItemType.KeycardFacilityManager },
                    MaxAmount = 1
                },


                new()
                {
                    Id = 2,
                    Price = 7000,
                    IgnoreFullInventory = false,
                    Name = "Logicer",
                    ItemTypes = new List<ItemType> { ItemType.GunLogicer },
                    MaxAmount = 1
                },


                new()
                {
                    Id = 3,
                    Price = 2500,
                    IgnoreFullInventory = false,
                    Name = "Shotgun",
                    ItemTypes = new List<ItemType> { ItemType.GunShotgun },
                    MaxAmount = 1
                },


                new()
                {
                    Id = 4,
                    Price = 3500,
                    IgnoreFullInventory = false,
                    Name = "Schwere Brustpanzerung",
                    ItemTypes = new List<ItemType> { ItemType.ArmorHeavy },
                    MaxAmount = 1
                },


                new()
                {
                    Id = 5,
                    Price = 500,
                    Name = "Schmerzmittel",
                    IgnoreFullInventory = false,
                    ItemTypes = new List<ItemType> { ItemType.Painkillers },
                    MaxAmount = 2
                },

                new()
                {
                    Id = 6,
                    Price = 1000,
                    Name = "Granate",
                    IgnoreFullInventory = false,
                    ItemTypes = new List<ItemType> { ItemType.GrenadeHE },
                    MaxAmount = 2
                },


                new()
                {
                    Id = 7,
                    Price = 750,
                    Name = "Flash Granate",
                    IgnoreFullInventory = false,
                    ItemTypes = new List<ItemType> { ItemType.GrenadeFlash },
                    MaxAmount = 2
                },

                new()
                {
                    Id = 8,
                    Price = 50000,
                    Name = "X3-Particle-Disruptor",
                    IgnoreFullInventory = false,
                    ItemTypes = new List<ItemType> { ItemType.ParticleDisruptor },
                    MaxAmount = 1
                }
            }
        },
        new Category
        {
            id = 6, AllowedRoles = new List<RoleTypeId> { RoleTypeId.None }, Name = "Allgemein",
            Description = "Hier findest du allgemeine Sachen.",
            Items = new List<GameStoreItemPrice>
            {
                new()
                {
                    Id = 1,
                    Price = 300,
                    IgnoreFullInventory = false,
                    Name = "Coin",
                    ItemTypes = new List<ItemType> { ItemType.Coin },
                    MaxAmount = 5
                },

                new()
                {
                    Id = 2,
                    Price = 350,
                    IgnoreFullInventory = false,
                    Name = "Flashlight",
                    ItemTypes = new List<ItemType> { ItemType.Flashlight },
                    MaxAmount = 5
                },


                new()
                {
                    Id = 3,
                    Price = 1000,
                    IgnoreFullInventory = false,
                    Name = "SCP-207",
                    ItemTypes = new List<ItemType> { ItemType.SCP207 },
                    MaxAmount = 2
                },


                new()
                {
                    Id = 4,
                    Price = 450,
                    IgnoreFullInventory = false,
                    Name = "Adrenalin",
                    ItemTypes = new List<ItemType> { ItemType.Adrenaline },
                    MaxAmount = 2
                },

                new()
                {
                    Id = 5,
                    Price = 500,
                    IgnoreFullInventory = false,
                    Name = "Medkit",
                    ItemTypes = new List<ItemType> { ItemType.Medkit },
                    MaxAmount = 3
                }
            }
        },
        new Category
        {
            id = 7, AllowedRoles = new List<RoleTypeId> { RoleTypeId.None },
            Name = "Munition", Description = "Hier findest du Munition",
            Items = new List<GameStoreItemPrice>
            {
                new()
                {
                    Id = 1,
                    Price = 0,
                    IgnoreFullInventory = true,
                    Name = "Kostenlose Munition",
                    AmmoTypes = new Dictionary<AmmoType, ushort>
                    {
                        { AmmoType.Nato762, 60 }, { AmmoType.Ammo44Cal, 12 }, { AmmoType.Nato556, 60 },
                        { AmmoType.Ammo12Gauge, 12 }, { AmmoType.Nato9, 60 }
                    },
                    MaxAmount = 1,
                    IsAmmo = true
                },

                new()
                {
                    Id = 2,
                    Price = 75,
                    IgnoreFullInventory = true,
                    Name = "9x19mm Munition",
                    AmmoTypes = new Dictionary<AmmoType, ushort> { { AmmoType.Nato9, 60 } },
                    MaxAmount = 2,
                    IsAmmo = true
                },
                new()
                {
                    Id = 3,
                    Price = 75,
                    IgnoreFullInventory = true,
                    Name = "12/70 Buckshot",
                    AmmoTypes = new Dictionary<AmmoType, ushort> { { AmmoType.Ammo12Gauge, 60 } },
                    MaxAmount = 2,
                    IsAmmo = true
                },
                new()
                {
                    Id = 4,
                    Price = 75,
                    IgnoreFullInventory = true,
                    Name = ".44 Mag",
                    AmmoTypes = new Dictionary<AmmoType, ushort> { { AmmoType.Ammo44Cal, 60 } },
                    MaxAmount = 2,
                    IsAmmo = true
                },
                new()
                {
                    Id = 6,
                    Price = 75,
                    IgnoreFullInventory = true,
                    Name = "7.62x39mm Munition",
                    AmmoTypes = new Dictionary<AmmoType, ushort> { { AmmoType.Nato762, 60 } },
                    MaxAmount = 2,
                    IsAmmo = true
                }
            }
        }
    };


}