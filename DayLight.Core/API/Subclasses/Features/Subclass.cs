using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API;
using JetBrains.Annotations;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DayLight.Core.API.Subclasses.Features;

public abstract class Subclass : Exiled.CustomRoles.API.Features.CustomRole
{
    public abstract int Chance { get; set; }
    public virtual int MaxSpawned { get; set; } = 100;

    public override bool RemovalKillsPlayer { get; set; } = false;
    public override int MaxHealth { get; set; } = 0;

    protected virtual RoleTypeId? SpawningRole { get; set; } = null;
    protected virtual bool Hidden { get; set; }= false;
    protected virtual float MaxHumeShield { get; set; } = 0;

    internal int Spawned = 0;

    [CanBeNull]
    public static Subclass GetSubclass(Player player)
    {
        var customRoleList = Registered.Where(customRole => customRole.Check(player)).ToList();
        if (customRoleList.Count == 0)
            return null;
        return customRoleList.First() as Subclass;

    }
    public override void AddRole(Player player)
    {
        if(Spawned >= MaxSpawned) return;
        Spawned++;
        Logger.Debug($"{Name}: Adding role to {player.Nickname}.");
        var UseDefaultSpawn = SpawnProperties.Count() == 0;
        if (Role != RoleTypeId.None)
        {
            switch (UseDefaultSpawn)
            {
                case false when KeepInventoryOnSpawn:
                    player.Role.Set(Role, SpawnReason.ForceClass, RoleSpawnFlags.AssignInventory);
                    break;
                case true when KeepInventoryOnSpawn:
                    player.Role.Set(Role, SpawnReason.ForceClass, RoleSpawnFlags.All);
                    break;
                case true when !KeepInventoryOnSpawn:
                    player.Role.Set(Role, SpawnReason.ForceClass, RoleSpawnFlags.UseSpawnpoint);
                    break;
                case false when !KeepInventoryOnSpawn:
                    player.Role.Set(Role, SpawnReason.ForceClass, RoleSpawnFlags.None);
                    break;
            }
        }

        TrackedPlayers.Add(player);

        if (SpawnProperties.Count() != 0)
            player.Position = GetSpawnPosition();
        if (!KeepInventoryOnSpawn)
        {
            Logger.Debug($"{Name}: Clearing {player.Nickname}'s inventory.");
            player.ClearInventory();
        }

        Timing.RunCoroutine(Utils.TryAddInventory(player, Inventory, this));

        foreach (var ammo in Ammo.Keys)
        {
            Logger.Debug($"{Name}: Adding {Ammo[ammo]} {ammo} to inventory.");
            player.SetAmmo(ammo, Ammo[ammo]);
        }

        Logger.Debug($"{Name}: Setting health values.");
        if (MaxHealth != 0)
        {
            player.Health = MaxHealth;
            player.MaxHealth = MaxHealth;
        }

        if (MaxHumeShield != 0)
        {
            player.ArtificialHealth = MaxHumeShield;
            player.MaxArtificialHealth = MaxHumeShield;
        }

        player.Scale = Scale;


        if (!Hidden)
        {
            Logger.Debug($"{Name}: Setting player info");
            player.CustomInfo = CustomInfo;
        }
        if (CustomAbilities == null) return;
        foreach (var ability in CustomAbilities)
            ability.AddAbility(player);
        ShowMessage(player);
        RoleAdded(player);
        player.UniqueRole = Name;
        player.TryAddCustomRoleFriendlyFire(Name, CustomRoleFFMultiplier);

        if (string.IsNullOrEmpty(ConsoleMessage)) return;
        var builder = StringBuilderPool.Pool.Get();

        builder.AppendLine(Name);
        builder.AppendLine(Description);
        builder.AppendLine();
        builder.AppendLine("Du bist als eine Subclass gespawnt!");

        if (CustomAbilities?.Count > 0)
        {
            builder.AppendLine("Deine Abilities sind:");
            for (var i = 1; i < CustomAbilities.Count + 1; i++)
                builder.AppendLine($"{i}. {CustomAbilities[i - 1].Name} - {CustomAbilities[i - 1].Description}");

            builder.AppendLine(
                "Du kannst mit \"cmdbind <Taste> .special\" eine Taste für deine Ability festlegen.");
        }

        player.SendConsoleMessage(StringBuilderPool.Pool.ToStringReturn(builder), "green");
    }

    protected override void ShowMessage(Player player)
    {
        player.ClearHint(ScreenZone.SubclassAlert);

        player.SendHint(ScreenZone.SubclassAlert,
            $"<size=20>Öffne deine Konsole (ö) für mehr Informationen zu deiner Subklasse</size>", 15);
        
    }
    [PublicAPI]
    public TimeSpan GetAbilityCooldown(Player player)
    {
        if (player.GetCustomRoles().Count(x => x.CustomAbilities != null && x.CustomAbilities.Count != 0) == 0)
        {
            return TimeSpan.MinValue;
        }
        var memberInfo = CustomAbilities?.First().GetType().BaseType;
        if (memberInfo != null && memberInfo.ToString() == "Subclasses.Features.ActiveAbility")
        {
            var selectedAbility = player.GetSelectedAbility();
            if (selectedAbility == null) return TimeSpan.MinValue;

            if (!selectedAbility.LastUsed.ContainsKey(player))
                return TimeSpan.Zero;
            var lastUsedTime = selectedAbility.LastUsed[player];
            var elapsedSinceLastUse = DateTime.Now - lastUsedTime;
            var remainingCooldown = TimeSpan.FromSeconds(selectedAbility.Cooldown) - elapsedSinceLastUse;

            if (remainingCooldown > TimeSpan.Zero)
                return remainingCooldown;
            else
                return TimeSpan.Zero;
        }
        else
        {
            if (memberInfo == null) return TimeSpan.MinValue;
            var ability = (PassiveAbility)CustomAbilities?.First();
            if (ability == null) return TimeSpan.MinValue;
            
            if(ability.EnableCooldown == false) return TimeSpan.MinValue;
            
            if (!ability.LastUsed.ContainsKey(player))
                return TimeSpan.Zero;

            var lastUsedTime = ability.LastUsed[player];
            var elapsedSinceLastUse = DateTime.Now - lastUsedTime;
            var remainingCooldown = TimeSpan.FromSeconds(ability.Cooldown) - elapsedSinceLastUse;

            if (remainingCooldown > TimeSpan.Zero)
                return remainingCooldown;
            
            
            return TimeSpan.Zero;

        }
    }
    public static IEnumerable<Subclass> RegisterSubclasses(bool skipReflection = false, object overrideClass = null, bool inheritAttributes = true, Assembly assembly = null)
    {
        var customRoleList = new List<Subclass>();
        Logger.Warn("Registering subclasses...");
        assembly ??= Assembly.GetCallingAssembly();
        foreach (var type in assembly.GetTypes())
        {
            if (type.BaseType != typeof (Subclass) && type.GetCustomAttribute(typeof (CustomRoleAttribute), inheritAttributes) == null)
            {
            }
            else
            { 
                foreach (var attribute in type.GetCustomAttributes(typeof (CustomRoleAttribute), inheritAttributes).Cast<Attribute>())
                {
                    var subclass = (Subclass) null;
                    if (!skipReflection && Server.PluginAssemblies.TryGetValue(assembly, out var plugin))
                    {
                        var pluginAssembly = Server.PluginAssemblies[assembly];
                          
                        foreach (var propertyInfo in overrideClass?.GetType().GetProperties() ?? pluginAssembly.Config.GetType().GetProperties())
                        {

                            if (propertyInfo.PropertyType != type)
                                continue;
                            subclass = propertyInfo.GetValue(overrideClass ?? (object) pluginAssembly.Config) as Subclass;
                            break;
                        }
                    }
                    subclass ??= (Subclass)Activator.CreateInstance(type);
                      
                      
                    if (subclass.Role == RoleTypeId.None)
                        subclass.Role = ((CustomRoleAttribute) attribute).RoleTypeId;
                      
                    if (!subclass.TryRegister())
                        continue;
                      
                    customRoleList.Add(subclass);

                    var roleTypeId = subclass.Role;
                    if (subclass.SpawningRole != null)
                        roleTypeId = (RoleTypeId)subclass.SpawningRole;
                    Manager.Subclasses.Add(new Manager.Subclass(subclass.Name, roleTypeId,
                        subclass.Chance,
                        subclass.MaxSpawned, 0));
                } 
            }
        }
        return customRoleList;
    }
    private bool TryRegister()
    {
        if (!Exiled.CustomRoles.CustomRoles.Instance.Config.IsEnabled)
            return false;
        if (!Registered.Contains(this))
        {
            if (Registered.Any((Func<Exiled.CustomRoles.API.Features.CustomRole, bool>) (r => (int) r.Id == (int) Id)))
            {
                Logger.Warn($"{(object)Name} has tried to register with the same Role ID as another role: {(object)Id}. It will not be registered!");
                return false;
            }
            Registered.Add(this);
            Init();
            Logger.Debug($"{(object)Name} ({(object)Id}) has been successfully registered.");
            return true;
        }
        Logger.Warn($"Couldn't register {(object)Name} ({(object)Id}) [{(object)Role}] as it already exists.");
        return false;
    }


}