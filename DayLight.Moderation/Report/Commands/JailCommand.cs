#region

using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using MEC;
using Neuron.Core;
using System;

#endregion

namespace DayLight.Moderation.Report.Commands;

internal class JailCommand : ICommand
{
    public string Command { get; } = "jail";

    public string[] Aliases { get; } = Array.Empty<string>();

    public string Description { get; } = "Usage: Jail <player> <Jail ID>";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("ws.jail"))
        {
            response = "You do not have permission to use this command";
            return false;
        }
        Player player;
        var pos = ModerationSystemPlugin.Instance.Config.Towers[0];
        switch (arguments.Count)
        {
            case 0:
                player = Player.Get(sender);
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