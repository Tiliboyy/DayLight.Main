using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace DayLight.Core.Subclasses.Features;

public abstract class PassiveAbility : Exiled.CustomRoles.API.Features.PassiveAbility
{
    public virtual int Cooldown { get; set; } = -1;
    protected virtual bool Broadcast { get; set; } = true;

    public virtual bool EnableCooldown { get; set; } = false;
    [YamlIgnore]

    public Dictionary<Player, DateTime> LastUsed = new Dictionary<Player, DateTime>();


    protected void UseAbility(Player player)
    {
        LastUsed[player] = DateTime.Now;
        player.ClearHint(ScreenZone.SubclassAlert);
        player.SendHint(
            ScreenZone.SubclassAlert,$"Ability {Name} wurde aktiviert!",
            Exiled.CustomRoles.CustomRoles.Instance.Config.UsedAbilityHint.Duration);
    }

    protected bool CanUseAbility(Player player)
    {
        if (Cooldown == -1 || !EnableCooldown)
            return true;
        if (!LastUsed.ContainsKey(player))
        {
            LastUsed.Add(player, DateTime.Now);
            return true;
        }

        var dateTime = this.LastUsed[player] + TimeSpan.FromSeconds(this.Cooldown);
        if (DateTime.Now > dateTime)
        {
            return true;
        }
        
        return false;
    }
}