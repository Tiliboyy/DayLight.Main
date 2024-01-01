using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;

namespace DayLight.Sync.Commands.RemoteAdmin.Subcommands;

internal class Toggle : CustomCommand
{
    public override string Command { get; } = "toggle";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Turn Stats on or off";
    public override string Permission { get; } = "DiscordSync.toggle";

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult commandResult)
    {

        if (!DayLightSyncPlugin.DisableDiscordSyncStats)
        {
            commandResult.Response = "Stats wurde deaktiviert";
            DayLightSyncPlugin.DisableDiscordSyncStats = true;
            return;
        }
        commandResult.Response = "Stats wurde aktiviert";
        DayLightSyncPlugin.DisableDiscordSyncStats = false;
    }
}
