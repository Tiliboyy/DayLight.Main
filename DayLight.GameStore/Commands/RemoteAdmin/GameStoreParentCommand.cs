using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using DayLight.GameStore.Commands.RemoteAdmin.SubCommands;
using Neuron.Core.Meta;

namespace DayLight.GameStore.Commands.RemoteAdmin;

[Automatic]
[Command(new [] { Platform.RemoteAdmin })]
public class GameStoreParentCommand : CustomParentCommand
{
    public override string Command => "gamestore";

    public override string[] Aliases { get; } = { "gs" };

    public override string Description => "The Gamestore parent command";

    protected override void RegisterCommands()
    {
        RegisterCommand(new Add());
        RegisterCommand(new Set());
        RegisterCommand(new Toggle());
        RegisterCommand(new ResetLimits());
        RegisterCommand(new Multiplier());
    }
}
