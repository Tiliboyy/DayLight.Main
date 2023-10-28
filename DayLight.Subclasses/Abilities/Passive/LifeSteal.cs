using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace DayLight.Subclasses.Abilities.Passive;

public class LifeSteal : DayLight.Core.Subclasses.Features.PassiveAbility
{
    public override string Name { get; set; } = "Life Steal";
    public override string Description { get; set; } = "Du heilst dich wenn du eine Person tötest.";

    protected override void AbilityAdded(Player player)
    {
        Exiled.Events.Handlers.Player.Died += OnKill;
    }

    private void OnKill(DiedEventArgs ev)
    {
        if (!Check(ev.Attacker)) return;
        ev.Attacker.Heal(100);
    }


    protected override void AbilityRemoved(Player player)
    {
        Exiled.Events.Handlers.Player.Died -= OnKill;
    }
}