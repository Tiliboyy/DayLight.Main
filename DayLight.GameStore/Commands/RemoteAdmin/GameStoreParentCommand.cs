using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.GameStore.Commands.RemoteAdmin.SubCommands;
using Exiled.Permissions.Extensions;
using Neuron.Core.Meta;
using System;
using System.Linq;

namespace DayLight.GameStore.Commands.RemoteAdmin;

[Automatic]
[Command(new [] { Platform.RemoteAdmin })]
public class GameStoreParentCommand : ParentCommand
{
    public GameStoreParentCommand() => LoadGeneratedCommands();

    public override string Command => "gamestore";

    public override string[] Aliases { get; } = { "gs" };

    public override string Description => "The Gamestore parent command";

    public override void LoadGeneratedCommands()
    {
        RegisterCommand(new Add());
        RegisterCommand(new Set());
        RegisterCommand(new Toggle());
        RegisterCommand(new ResetLimits());
        RegisterCommand(new Multiplier());
    }

    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = AllCommands.Where(command => sender.CheckPermission($"gs.{command.Command}")).Aggregate("\nPlease enter a valid subcommand:", (current, command) => current + $"\n\n<color=#00fce3><b>- {command.Command} </b></color>\n<color=white>{command.Description}</color>");
        return false;
    }
}
