using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;

namespace DayLight.Subclasses.Abilities.Passive;

public class ExplodeOnDeath : DayLight.Core.Subclasses.Features.PassiveAbility
{
    public override string Name { get; set; } = "Boom";
    public override string Description { get; set; } = "Du explodierst wenn du stirbst.";

    protected override void AbilityAdded(Player player)
    {
        Exiled.Events.Handlers.Player.Dying += OnDied;
    }

    protected override void AbilityRemoved(Player player)
    {
        Exiled.Events.Handlers.Player.Dying -= OnDied;
    }

    public void OnDied(DyingEventArgs ev)
    {
        if (!Check(ev.Player))
            return;
        var grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE, ev.Player);
        grenade.FuseTime = 0.5f;
        grenade.SpawnActive(ev.Player.Position, ev.Player);
    }
}