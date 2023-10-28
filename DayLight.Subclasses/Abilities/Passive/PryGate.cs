using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;

namespace DayLight.Subclasses.Abilities.Passive;

public class PryGate : DayLight.Core.Subclasses.Features.PassiveAbility
{
    public override string Name { get; set; } = "Pry Gate";
    public override string Description { get; set; } = "Du kannst alle 60 Sekunden eine Tür aufreissen.";
    public override bool EnableCooldown { get; set; } = true;
    public override int Cooldown { get; set; } = 60;

    protected override void AbilityAdded(Player player)
    {
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
    }

    protected override void AbilityRemoved(Player player)
    {
        Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
    }

    private void OnInteractingDoor(InteractingDoorEventArgs ev)
    {

        if (!Check(ev.Player))
            return;

        if( !ev.Door.IsGate|| ev.Door.IsOpen) return;

        if (ev.Door.Type is DoorType.Scp079Second or DoorType.Scp079First)
            return;
        if (!CanUseAbility(ev.Player))
            return;
        UseAbility(ev.Player);

        ((Gate)ev.Door).TryPry();
    }
}