using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace DayLight.Subclasses.Abilities.Passive;

public class No207Damage : DayLight.Core.Subclasses.Features.PassiveAbility
{
    public override string Name { get; set; } = "Immunity";
    public override string Description { get; set; } = "Du nimmst keinen Schaden von SCP-207.";

    protected override void AbilityAdded(Player player)
    {
        Exiled.Events.Handlers.Player.Hurting += OnHurt;
    }

    protected override void AbilityRemoved(Player player)
    {
        LastUsed.Remove(player);
        Exiled.Events.Handlers.Player.Hurting -= OnHurt;
    }


    public void OnHurt(HurtingEventArgs ev)
    {
        if (!Check(ev.Player)) return;
        if (ev.DamageHandler.Type != DamageType.Scp207) return;
        ev.IsAllowed = false;
    }
}