using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace DayLight.Subclasses.Abilities.Passive;

public class SpeedOnKill : DayLight.Core.Subclasses.Features.PassiveAbility
{
    public override string Name { get; set; } = "Schneller Tot";
    public override string Description { get; set; } = "Du wirst bei jedem Kill schneller.";

    protected override void AbilityAdded(Player player)
    {
        Exiled.Events.Handlers.Player.Died += OnDied;
    }

    protected override void AbilityRemoved(Player player)
    {
        Exiled.Events.Handlers.Player.Died -= OnDied;
    }

    public void OnDied(DiedEventArgs ev)
    {
        if (!Check(ev.Attacker))
            return;
        if (ev.Attacker.TryGetEffect(EffectType.MovementBoost, out var effect))
        {
            if (effect.Intensity > 100) return;
            var e = (byte)(effect.Intensity + 10);
            ev.Attacker.ChangeEffectIntensity<MovementBoost>(e);
        }
        else
        {
            ev.Attacker.EnableEffect<MovementBoost>();
            ev.Attacker.ChangeEffectIntensity<MovementBoost>(10);
        }
    }
}