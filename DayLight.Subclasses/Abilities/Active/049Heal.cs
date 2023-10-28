using Core.Features.Data.Enums;
using Core.Features.Extensions;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using System.Linq;
using UnityEngine;

namespace DayLight.Subclasses.Abilities.Active;

public class SCP049Heal : ActiveAbility
{
    public override string Name { get; set; } = "Heilung";
    public override string Description { get; set; } = "Du heilst alle deine Teammates in deiner nähe.";


    protected override void AbilityUsed(Player player)
    {
        int count = 0;
        foreach (var scp in Player.List.Where(x => x.IsScp))
        {
            if (!(Vector3.Distance(player.Position, scp.Position) < 5)) continue;
            scp.Heal(300);
            count++;
        }

        player.SendHint(ScreenZone.Notifications, "Du hast " + count + " SCPs geheilt!",4);

        base.AbilityUsed(player);
    }

    protected override void AbilityRemoved(Player player)
    {
        base.AbilityUsed(player);
    }

    public override float Duration { get; set; } = 0;
    public override float Cooldown { get; set; } = 180;
}