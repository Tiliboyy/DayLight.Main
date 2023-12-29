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

    public override string Permission { get; } = "subclasses.toggle";
    protected override void Respond(ArraySegment<string> arguments, Player sender, ref CommandResult response)
    {

        if (!Manager.NoRandomRole)
        {
            Manager.NoRandomRole = true;
            response.Response = $"Disabled Subclasses";
        }
        else
        {
            Manager.NoRandomRole = false;
            response.Response = $"Enabled Subclasses";
        }

        response.Success = true;
    }
}