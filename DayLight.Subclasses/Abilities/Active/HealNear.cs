using Core.Features.Data.Enums;
using Core.Features.Extensions;
using DayLight.Core.Subclasses.Features;
using Exiled.API.Features;
using UnityEngine;

namespace DayLight.Subclasses.Abilities.Active;

public class HealNear : ActiveAbility
{
    public override string Name { get; set; } = "Heal";

    public override string Description { get; set; } =
        "Heilt alle Teammitglieder in deiner nähe voll.";

    public override float Duration { get; set; } = 1f;
    public override float Cooldown { get; set; } = 600f;

    protected override void AbilityUsed(Player player)
    {
        foreach (var ply in Player.List)
        {
            if (ply.Role.Team != player.Role.Team ||
                !(Vector3.Distance(ply.Transform.position, player.Transform.position) < 5)) continue;
            ply.Health = ply.MaxHealth;
            ply.SendHint(ScreenZone.Notifications, "Du wurdest von " + player.Nickname + " geheilt!", 4);
        }

        base.AbilityUsed(player);
    }
}