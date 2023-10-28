using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace DayLight.Subclasses.Abilities.Passive;

public class Hack : DayLight.Core.Subclasses.Features.PassiveAbility
{
    public override string Name { get; set; } = "Hack";
    public override string Description { get; set; } = "Du kannst alle 60 Sekunden eine Tür hacken.";
    public override bool EnableCooldown { get; set; } = true;

    public override int Cooldown { get; set; } = 50;

    protected override void AbilityAdded(Player player)
    {
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
    }

    protected override void AbilityRemoved (Player player)
    {
        Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
    }

    private void OnInteractingDoor(InteractingDoorEventArgs ev)
    {
        if (ev.IsAllowed || (ev.Door.IsOpen && !ev.Door.IsFullyOpen)) return;
        if (ev.Door.Type is DoorType.Scp079Second or DoorType.Scp079First)
        {
            ev.Player.ShowHint("Diese Tür wird von SCP-079 von deinen Hacks geschützt!");
            return;
        }
        if (!Check(ev.Player))
            return;




        if (!ev.Door.IsKeycardDoor || ev.IsAllowed) return;

        if (!CanUseAbility(ev.Player))
            return;
        UseAbility(ev.Player);
        ev.IsAllowed = true;
    }
}