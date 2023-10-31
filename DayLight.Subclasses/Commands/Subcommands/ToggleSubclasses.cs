using CommandSystem;
using DayLight.Core.API.CommandSystem;
using DayLight.Core.API.Subclasses;
using Exiled.API.Features;
using System;

namespace DayLight.Subclasses.Commands.Subcommands;
[CommandHandler(typeof(SubclassParentCommand))]

internal class Toggle : CustomCommand
{
    public override string Command { get; } = "toggle";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Enable/Disable subclasses";

    protected override string Permission { get; } = "subclasses.toggle";
    protected override bool Respond(ArraySegment<string> arguments, Player sender, out string response)
    {

        if (!Manager.NoRandomRole)
        {
            Manager.NoRandomRole = true;
            response = $"Disabled Subclasses";
        }
        else
        {
            Manager.NoRandomRole = false;
            response = $"Enabled Subclasses";
        }

        return true;
    }
}