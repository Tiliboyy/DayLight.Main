using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Neuron.Core.Meta;

namespace DiscordSync.Plugin.Commands.ClientConsole;

[Automatic]
[Command(new [] { Platform.ClientConsole })]
internal class UnLink : CustomCommand
{
    public override string Command { get; } = "unlink";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Unlinkt deinen Discord Account";
    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult response)
    {
        ulong.TryParse(player.RawUserId, out var result);
        response.Response = Link.LinkDatabase.Unlink(result) ? "Unlinked!" : "Du bist nicht verlinkt!";
        
    }

}
