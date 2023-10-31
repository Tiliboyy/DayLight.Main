using Core.Features.Data.Enums;
using Core.Features.Extensions;
using DayLight.Core.API.Subclasses.Features;
using Exiled.API.Features;
using PlayerRoles;
using System;
using System.Linq;
using System.Text;

namespace DayLight.Subclasses.Abilities.Active;

public class SCPScan : ActiveAbility
{
    public override string Name { get; set; } = "SCP Scan";

    public override string Description { get; set; } =
        "Gibt dir Informationen über einen zufälligen SCP aus.";

    public override float Duration { get; set; } = 1f;
    public override float Cooldown { get; set; } = 60f;

    protected override void AbilityUsed(Player player)
    {
        var scps = Player.List.Where(x => x.IsScp && x.Role.Type != RoleTypeId.Scp0492 && x.Role.Type != RoleTypeId.Scp079).ToList();
        if (scps.Count == 0)
        {
            player.SendHint(ScreenZone.CenterBottom, "Es konnte kein SCP gefudnen werden", 2);
            return;
        }
        var ply = scps.RandomItem();
        
        StringBuilder scplistbuilder = new();



        scplistbuilder.Append(ply.Nickname);
        scplistbuilder.Append(" | ");
        scplistbuilder.Append(Core.API.Subclasses.Utils.GetSCPRoleName(ply));
        scplistbuilder.Append(" | ");


        scplistbuilder.Append("<color=green>AHP: ");
        scplistbuilder.Append((int)Math.Round((double)(100 * ply.HumeShield) /
                                              ply.HumeShieldStat.MaxValue));
        scplistbuilder.Append("%</color> | ");
        scplistbuilder.Append("<color=");
        scplistbuilder.Append(Core.API.Subclasses.Utils.GetHPColor(ply));
        scplistbuilder.Append(">HP: ");
        scplistbuilder.Append(
            (int)Math.Round((double)(100 * ply.Health) / ply.MaxHealth));
        scplistbuilder.Append("%</color> | ");

        scplistbuilder.Append("Zone: " + Core.API.Subclasses.Utils.GetZoneName(ply.Zone));

        player.SendHint(ScreenZone.CenterBottom, scplistbuilder.ToString(), 10);
        base.AbilityUsed(player);
    }
    
}