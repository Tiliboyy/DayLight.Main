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

    protected override string Permission { get; } = "PlayerHints.Toggle";

    protected override bool Respond(ArraySegment<string> arguments, Player sender, out string response)
    {

        if (PlayerHintsPlugin.DisableScpList)
        {
            PlayerHintsPlugin.DisableScpList = false;
            response = $"Enabled!";
        }
        else
        {
            PlayerHintsPlugin.DisableScpList = true;
            response = $"Disabled!";
        }
        return true;
    }

}