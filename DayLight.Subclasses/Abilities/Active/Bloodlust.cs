using CustomPlayerEffects;
using DayLight.Core.Subclasses.Features;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System.Collections.Generic;

namespace DayLight.Subclasses.Abilities.Active;

public class Bloodlust : ActiveAbility
{
    public override string Name { get; set; } = "Bloodlust";

    public override string Description { get; set; } =
        "Gibt dir einen temporären Boost und lässt dich bei einem Kill heilen, aber danach lässt es dich geschwächt zurück.";

    public override float Duration { get; set; } = 10;
    public override float Cooldown { get; set; } = 1;

    protected override void AbilityUsed(Player player)
    {
        Exiled.Events.Handlers.Player.Hurting += OnTakingDamage;
        Exiled.Events.Handlers.Player.Died += OnDied;
        player.DisableEffect<Scp207>();
        player.EnableEffect<Deafened>();
        player.EnableEffect<CardiacArrest>();
        player.EnableEffect<Scp1853>();
        player.ChangeEffectIntensity<Scp1853>(4);
        player.EnableEffect<MovementBoost>();
        player.ChangeEffectIntensity<MovementBoost>(50);
        base.AbilityUsed(player);
    }

    protected override void AbilityEnded(Player player)
    {
        Exiled.Events.Handlers.Player.Hurting -= OnTakingDamage;
        Exiled.Events.Handlers.Player.Died -= OnDied;
        Timing.RunCoroutine(Damage(player), player.Nickname);
        player.DisableEffect<Deafened>();
        player.DisableEffect<Scp1853>();
        player.DisableEffect<CardiacArrest>();
        player.DisableEffect<MovementBoost>();
        player.EnableEffect<Sinkhole>(10f);
        base.AbilityEnded(player);
    }

    public static IEnumerator<float> Damage(Player player)
    {
        for (int  i = 0; i < player.Health/10; i += 1)
        {
            player.Hurt(1, DamageType.Bleeding);
            yield return Timing.WaitForSeconds(0.1f);
        }
    }
    public void OnDied(DiedEventArgs ev)
    {
        if (!Check(ev.Attacker)) return;
        ev.Attacker.Heal(20);
    }

    public void OnTakingDamage(HurtingEventArgs ev)
    {
        if (!Check(ev.Player)) return;

        if (ev.DamageHandler.Type == DamageType.CardiacArrest)
            ev.DamageHandler.Damage = 0;
    }
}