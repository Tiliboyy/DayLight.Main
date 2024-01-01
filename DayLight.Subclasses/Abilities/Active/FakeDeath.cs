using DayLight.Core.API;
using DayLight.Core.API.Subclasses.Features;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items;
using MEC;
using PlayerStatsSystem;
using System;

namespace DayLight.Subclasses.Abilities.Active;

public class FakeDeath : ActiveAbility
{
    public override string Name { get; set; } = "Fake";

    public override string Description { get; set; } =
        "Fälscht deinen Tot.";

    public override float Duration { get; set; } = 15;
    public override float Cooldown { get; set; } = 60f;

    protected override void AbilityUsed(Player player)
    {
        try
        {
            Exiled.Events.Handlers.Player.ChangingItem += OnChangingItem;
            player.EnableEffect(EffectType.Invisible, Duration);
            player.EnableEffect(EffectType.Ensnared, Duration);
            player.Inventory.NetworkCurItem = ItemIdentifier.None;
            player.Inventory.CurInstance = null;
            var ragdoll = Ragdoll.CreateAndSpawn(player.Role.Type, player.Nickname,
                new UniversalDamageHandler(100, DeathTranslations.Unknown), player.Position, player.Transform.rotation,
                player);
            Timing.CallDelayed(Duration, () => { ragdoll.Destroy(); });
            base.AbilityUsed(player);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw;
        }
    }

    protected override void AbilityEnded(Player player)
    {
        Exiled.Events.Handlers.Player.ChangingItem -= OnChangingItem;
        player.DisableEffect(EffectType.Invisible);
        player.DisableEffect(EffectType.Ensnared);
        base.AbilityEnded(player);
    }

    public void OnChangingItem(ChangingItemEventArgs ev)
    {
        if (!Check(ev.Player)) return;
        ev.IsAllowed = false;
    }
}