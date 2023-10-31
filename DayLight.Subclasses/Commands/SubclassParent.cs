using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Subclasses.Commands.Subcommands;
using Neuron.Core.Meta;
using System;
using System.Linq;

namespace DayLight.Subclasses.Commands;
[Automatic]
[Command(new [] { Platform.RemoteAdmin })]
public class SubclassParentCommand : ParentCommand
{
    public SubclassParentCommand() => LoadGeneratedCommands();
    public override string Command { get; } = "subclasses";
    public override string[] Aliases { get; } = Array.Empty<string>();
    public override string Description { get; } = "Subclasses setttings";


    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = AllCommands.Aggregate("\nPlease enter a valid subcommand:", (current, command) => current + $"\n\n<color=yellow><b>- {command.Command} </b></color>\n<color=white>{command.Description}</color>");
        return false;
    }


    public override sealed void LoadGeneratedCommands()
    {
        RegisterCommand(new GenerateSubclassList());
        RegisterCommand(new Toggle());
        RegisterCommand(new ResetSubclassSpawns());
    }
    public string[] Usage { get; } = new[] { "<arguments>" };

}
