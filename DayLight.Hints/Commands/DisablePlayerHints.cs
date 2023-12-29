using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;

namespace DayLight.Hints.Commands;

[Command(new [] { Platform.RemoteAdmin })]
internal class TogglePlayerHints : CustomCommand
{
    public override string Command { get; } = "toggleplayerhints";

    public override string[] Aliases { get; } = {"tph"};

    public override string Description { get; } = "Toggles PlayerHints Hints";

    public override string Permission { get; } = "PlayerHints.Toggle";

    protected override void Respond(ArraySegment<string> arguments, Player sender, ref CommandResult commandResult)
    {

        if (PlayerHintsPlugin.DisableScpList)
        {
            PlayerHintsPlugin.DisableScpList = false;
            commandResult.Response = $"Enabled!";
        }
        else
        {
            PlayerHintsPlugin.DisableScpList = true;
            commandResult.Response = $"Disabled!";
        }
    }

}