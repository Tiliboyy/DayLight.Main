using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using System.Collections.Generic;
using Item = Exiled.API.Features.Items.Item;
using Player = Exiled.API.Features.Player;

namespace DayLight.Misc;

public class Eventhandlers
{
    public static void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        if (!MiscPlugin.EnableTeamRespawns)
        {
            ev.IsAllowed = false;
        }
    }
    public static void OnHurt(HurtingEventArgs ev)
    {
        if (ev.Attacker.Role != null && ev.Player != null && ev.DamageHandler != null && ev.Attacker != null && ev.DamageHandler.Type == DamageType.Scp018 && ev.Attacker.Role.Team == ev.Player.Role.Team && ev.Attacker != ev.Player)
            ev.IsAllowed = false;
    }
    public static void OnUsingItem(UsedItemEventArgs ev)
    {
        switch (ev.Item.Type)
        {
            case ItemType.SCP207:
            {
                var curlevel = ev.Player.GetEffect(EffectType.Scp207).Intensity;
                if (curlevel == 4 && MiscPlugin.Instance.Config.Scp207ExplosionAmount < 4)
                    curlevel += 1;
                    
                if (curlevel == MiscPlugin.Instance.Config.Scp207ExplosionAmount)
                {
                    for (int i = 0; i < MiscPlugin.Instance.Config.Scp207ExplosionGrenadeAmount; i++)
                    {
                        var grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                        var time = MiscPlugin.Instance.Config.FuseTime;
                        grenade.FuseTime = time;
                        grenade.SpawnActive(ev.Player.Transform.position, ev.Player);
                    }

                }

                break;
            }
            case ItemType.Adrenaline:
                if (MiscPlugin.Instance.Config.AdrenalinSpeedboost)
                {
                    var intensity = ev.Player.GetEffectIntensity<MovementBoost>();
                    if (MiscPlugin.Instance.Config.StackableSpeed)
                    {
                        if (intensity + MiscPlugin.Instance.Config.SpeedIntensity > MiscPlugin.Instance.Config.MaxStackedSpeed)
                        {
                            intensity = MiscPlugin.Instance.Config.MaxStackedSpeed;
                        }
                        else
                        {
                            intensity += MiscPlugin.Instance.Config.SpeedIntensity;
                        }
                    }
                    ev.Player.ChangeEffectIntensity<MovementBoost>(intensity,
                        MiscPlugin.Instance.Config.SpeedDuration);
                }
                
                break;
            default:
                break;
        }
    }


    public static void OnRoundEnd(RoundEndedEventArgs ev)
    {
        MiscPlugin.EnableTeamRespawns = true;
        if (!MiscPlugin.Instance.Config.AutoFriendlyFireToggle) return;
        var roles = new List<RoleTypeId>()
        {
            RoleTypeId.Scientist,
            RoleTypeId.Tutorial,
            RoleTypeId.ChaosConscript,
            RoleTypeId.ChaosMarauder,
            RoleTypeId.ChaosRepressor,
            RoleTypeId.ChaosRifleman,
            RoleTypeId.ChaosConscript,
            RoleTypeId.ClassD,
            RoleTypeId.FacilityGuard,
            RoleTypeId.NtfCaptain,
            RoleTypeId.NtfPrivate,
            RoleTypeId.NtfSergeant,
            RoleTypeId.NtfSpecialist,
        };
        foreach (var player in Player.List)
        {
            foreach (var role in roles)
            {
                player.TryAddFriendlyFire(role, 1);
            }
        }
    }
}