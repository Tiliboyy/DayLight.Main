#region

using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using DayLight.Moderation.Configs;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
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
    protected override string Permission { get; } = "ws.jail";
    protected override bool Respond(ArraySegment<string> arguments, Player player, out string response)
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
                    response = "Tower ID wurde nicht gefunden";
                    return true;
                }

                if (!Globals.Get<ModerationConfig>().Towers.ContainsKey(i))
                {
                    response = "Tower ID wurde nicht gefunden";
                    return true;
                }

                pos = ModerationSystemPlugin.Instance.Config.Towers[i];
                player = Player.Get(arguments.At(0));
                break;
            default:
                response = "Usage: Jail <player> <Jail ID>";
                return true;
        }

        if (player == null)
        {
            response = "Player not found";
            return true;
        }

        Timing.RunCoroutine(Jail.JailPlayer(player, pos));
        response = $"{player.Nickname} was Jailed";
        return true;
    }
}