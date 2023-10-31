using DayLight.Core.API.Subclasses.Features;
using Exiled.API.Features;

namespace DayLight.Subclasses.Abilities.Active;

public class Surge : ActiveAbility
{
    public override string Name { get; set; } = "Surge";
    public override string Description { get; set; } = "Macht das Licht in deinem Raum aus.";
    public override float Duration { get; set; } = 7;
    public override float Cooldown { get; set; } = 60;

    protected override void AbilityUsed(Player player)
    {
        player.CurrentRoom.TurnOffLights(Duration);
        base.AbilityUsed(player);

    }
}