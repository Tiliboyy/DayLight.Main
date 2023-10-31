using DayLight.Core.API.Subclasses.Features;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace DayLight.Subclasses.Abilities.Active;

public class Vent : ActiveAbility
{
    public override string Name { get; set; } = "Vent";

    public override string Description { get; set; } =
        "Du bist unsichtbar und kannst dich durch Türen teleportieren!";

    public override float Duration { get; set; } = 10;
    public override float Cooldown { get; set; } = 60f;

    protected override void AbilityUsed(Player player)
    {
        player.EnableEffect(EffectType.Invisible, Duration + 5);
        SubscribeEvent();
        Timing.CallDelayed(Duration, () =>
        {
            player.DisableEffect(EffectType.Invisible);
            UnsubscribeEvent();
        });
    }

    public void SubscribeEvent()
    {
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
    }

    public void UnsubscribeEvent()
    {
        Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
    }

    public void OnInteractingDoor(InteractingDoorEventArgs ev)
    {
        if (!Check(ev.Player)) return;
        if (ev.Door.Type is DoorType.Scp079Second or DoorType.Scp079First) return;

        ev.IsAllowed = false;
        ev.Player.EnableEffect(EffectType.Invisible, 20);
        var newPos = ev.Door.Position + ev.Player.GameObject.transform.forward;
        newPos.y = ev.Player.Position.y;
        ev.Player.Position = newPos;
    }
}