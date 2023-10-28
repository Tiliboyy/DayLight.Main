using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using System;

namespace DayLight.Core.Subclasses.Features;

public abstract class ActiveAbility : Exiled.CustomRoles.API.Features.ActiveAbility
{
    protected override void ShowMessage(Player player)
    {

        player.ClearHint(ScreenZone.SubclassAlert);
        player.SendHint(
            ScreenZone.SubclassAlert,$"Ability {Name} wurde aktiviert!",
            Exiled.CustomRoles.CustomRoles.Instance.Config.UsedAbilityHint.Duration);
    }
    

    public override bool CanUseAbility(Player player, out string response, bool selectedOnly = false)
    {
        if (this.CanUseOverride != null)
        {
            response = string.Empty;
            return this.CanUseOverride();
        }

        if (!this.LastUsed.ContainsKey(player))
        {
            response = string.Empty;
            return true;
        }
        var dateTime = this.LastUsed[player] + TimeSpan.FromSeconds(this.Cooldown);
        if (DateTime.Now > dateTime)
        {
            response = string.Empty;
            return true;
        }

        response =
            $"Du must noch {(object)Math.Round((dateTime - DateTime.Now).TotalSeconds)} Sekunden warten um {(object)this.Name} zu nutzen!";
        player.ClearBroadcasts();
        return false;

    }
    protected override void AbilityAdded(Player player)
    {
        SelectAbility(player);
        base.AbilityAdded(player);
    }
}