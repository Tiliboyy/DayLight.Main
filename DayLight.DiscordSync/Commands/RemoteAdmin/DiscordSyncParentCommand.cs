

using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using DiscordSync.Plugin.Commands.RemoteAdmin.Subcommands;
using Exiled.Permissions.Extensions;
using Neuron.Core.Meta;

namespace DiscordSync.Plugin.Commands.RemoteAdmin;
[Automatic]
[Command(new [] { Platform.RemoteAdmin })]
public class GameStoreParentCommand : CustomParentCommand
{
    public override string Command => "DiscordSync";

    public override string[] Aliases { get; } = { "ds" };

    public override string Description => "The DiscordSync parent command";
    
    protected override void RegisterCommands()
    {
        RegisterCommand(new SyncRoles());
        RegisterCommand(new Toggle());
    }
}
