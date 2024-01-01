#region

using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using DayLight.Moderation.Configs;
using Exiled.API.Features;
using MEC;
using Neuron.Core;
using Neuron.Core.Meta;
using System;

#endregion

namespace DayLight.Moderation.Report.Commands;

[Automatic]
[Command(new [] { Platform.RemoteAdmin })]
internal class JailCommand : CustomCommand
{
    public override string Command { get; } = "jail";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Usage: Jail <player> <Jail ID>";
    public override string Permission { get; } = "ws.jail";
    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult response)
    {

        var pos = ModerationSystemPlugin.Instance.Config.Towers[0];
        switch (arguments.Count)
        {
            case 0:
                break;
            case 1:
                player = Player.Get(arguments.At(0));
                break;
            case 2:
                if (!int.TryParse(arguments.At(1), out int i))
                {
                    response.Response = "Tower ID wurde nicht gefunden";
                    response.Success = false;
                    return;
                }

                if (!Globals.Get<ModerationConfig>().Towers.ContainsKey(i))
                {
                    response.Response= "Tower ID wurde nicht gefunden";
                    response.Success = false;
                    return;
                }

                pos = ModerationSystemPlugin.Instance.Config.Towers[i];
                player = Player.Get(arguments.At(0));
                break;
            default:
                response.Response = "Usage: Jail <player> <Jail ID>";
                response.Success = false;
                return;
        }

        if (player == null)
        {
            response.Response = "Player not found";
            response.Success = false;
            return;
        }

        Timing.RunCoroutine(Jail.JailPlayer(player, pos));
        response.Response = $"{player.Nickname} was Jailed";
        response.Success = true;

        return;
    }
}