using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using DayLight.Sync.Link;
using Exiled.API.Features;
using Neuron.Core.Meta;

namespace DayLight.Sync.Commands.ClientConsole;

[Automatic]
[Command(new [] { Platform.ClientConsole })]
internal class Unlink : CustomCommand
{
    public override string Command { get; } = "unlink";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Unlinkt deinen Discord Account";
    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult response)
    {
        ulong.TryParse(player.RawUserId, out var result);
        response.Response = LinkUtils.Unlink(result) ? "Unlinked!" : "Du bist nicht verlinkt!";
        
    }

}
