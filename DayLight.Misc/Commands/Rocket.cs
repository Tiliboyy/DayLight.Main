using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Permissions.Extensions;
using MEC;
using Neuron.Core.Meta;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Vector3 = UnityEngine.Vector3;


namespace DayLight.Misc.Commands;

[Automatic]
[Command(new [] { Platform.RemoteAdmin })]
internal class Rocket : CustomCommand
{
    public override string Command { get; } = "rocket";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Sends players high in the sky and explodes them";

    public override string Permission { get; } = "AT.rocket";
    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult commandResult)
    {
        if (arguments.Count != 2)
        {
            commandResult.Response = "Usage: rocket ((player id / name) or (all / *)) (speed)";
            commandResult.Success = false;
            return;
        }

        switch (arguments.At(0))
        {
            case "*":
            case "all":
                if (!float.TryParse(arguments.At(1), out var speed) && speed <= 0)
                {
                    commandResult.Response = $"Speed argument invalid: {arguments.At(1)}";
                    commandResult.Success = false;
                    return;
                }

                foreach (Player ply in Player.List)
                    Timing.RunCoroutine(DoRocket(ply, speed));

                commandResult.Response = "Everyone has been rocketed into the sky (We're going on a trip, in our favorite rocketship)";
                commandResult.Success = true;
                return;
            default:
                var pl = Player.Get(arguments.At(0));
                if (pl == null)
                {
                    commandResult.Response = $"Player not found: {arguments.At(0)}";
                    commandResult.Success = false;
                    return;
                }
                if (pl.Role == RoleTypeId.Spectator || pl.Role == RoleTypeId.None)
                {
                    commandResult.Response = $"Player {pl.Nickname} is not a valid class to rocket";
                    commandResult.Success = false;
                    return;
                }

                if (!float.TryParse(arguments.At(1), out float spd) && spd <= 0)
                {
                    commandResult.Response = $"Speed argument invalid: {arguments.At(1)}";
                    commandResult.Success = false;

                    return;
                }

                Timing.RunCoroutine(DoRocket(pl, spd));
                commandResult.Response = $"Player {pl.Nickname} has been rocketed into the sky (We're going on a trip, in our favorite rocketship)";
                commandResult.Success = true;
                return;
        }
    }
    public static IEnumerator<float> DoRocket(Player player, float speed)
    {
        const int maxAmnt = 50;
        int amnt = 0;
        while (player.Role != RoleTypeId.Spectator)
        {
            player.Position += Vector3.up * speed;
            amnt++;
            if (amnt >= maxAmnt)
            {
                player.IsGodModeEnabled = false;
                ExplosiveGrenade grenade = (ExplosiveGrenade)Item.Create(ItemType.GrenadeHE);
                grenade.FuseTime = 0.5f;
                grenade.SpawnActive(player.Position, player);
                player.Kill("Went on a trip in their favorite rocket ship.");
            }

            yield return Timing.WaitForOneFrame;
        }
    }
}