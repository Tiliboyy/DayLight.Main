using Core.Features.Data.Enums;
using Core.Features.Extensions;
using DayLight.Core.Subclasses.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;

namespace DayLight.Subclasses.Abilities.Passive;

public class SeeHP : PassiveAbility
{

    public override string Name { get; set; } = "Sense";
    public override string Description { get; set; } = "Du kannst die HP deiner gegner sehen.";

    protected override void SubscribeEvents()
    {
        Player.Hurting += OnHurting;
        base.SubscribeEvents();
    }

    protected override void UnsubscribeEvents()
    {
        Player.Hurting -= OnHurting;
        base.UnsubscribeEvents();
    }
    public void OnHurting(HurtingEventArgs ev)
    {
        if(Check(ev.Attacker))
            ev.Attacker.SendHint(ScreenZone.CenterBottom, ev.Player.Nickname + $" - <color={DayLight.Core.Subclasses.Utils.GetHPColor(ev.Player)}>" + ev.Player.Health + "HP</color>", 1);
    }

}
