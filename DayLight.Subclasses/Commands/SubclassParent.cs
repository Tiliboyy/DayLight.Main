using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using DayLight.Subclasses.Commands.Subcommands;
using Neuron.Core.Meta;
using System;
using System.Linq;

namespace DayLight.Subclasses.Commands;
[Automatic]
[Command(new [] { Platform.RemoteAdmin })]
public class SubclassParentCommand : CustomParentCommand
{
    public SubclassParentCommand() => LoadGeneratedCommands();
    public override string Command { get; } = "subclasses";
    public override string[] Aliases { get; } = Array.Empty<string>();
    public override string Description { get; } = "Subclasses setttings";
    protected override void RegisterCommands()
    {
        RegisterCommand(new GenerateSubclassList());
        RegisterCommand(new Toggle());
        RegisterCommand(new ResetSubclassSpawns());
    }

}
