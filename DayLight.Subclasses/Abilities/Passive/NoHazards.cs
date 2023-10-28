using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace DayLight.Subclasses.Abilities.Passive;

public class NoHazards : DayLight.Core.Subclasses.Features.PassiveAbility
{
    public override string Name { get; set; } = "Gut gehen";

    public override string Description { get; set; } =
        "Du wirst von Sinkholes oder SCP-173 Tantrums nicht verlangsamt.";


    protected override void AbilityAdded(Player player)
    {
        Exiled.Events.Handlers.Player.EnteringEnvironmentalHazard += OnEnteringHazard;
    }

    protected override void AbilityRemoved(Player player)
    {
        Exiled.Events.Handlers.Player.EnteringEnvironmentalHazard -= OnEnteringHazard;
        base.SubscribeEvents();
    }

    private void OnEnteringHazard(EnteringEnvironmentalHazardEventArgs ev)
    {
        if (!Check(ev.Player))
            return;
        ev.IsAllowed = false;
    }
}