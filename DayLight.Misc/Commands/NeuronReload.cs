using DayLight.Core.API;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Neuron.Core;
using Neuron.Core.Logging;
using Neuron.Core.Meta;
using Neuron.Modules.Reload;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DayLight.Misc.Commands;
[Automatic]
[Command(new [] { Platform.RemoteAdmin })]

public class NeuronReload : CustomCommand
{

    public override string Command { get; } = "NeuronReload";
    public override string[] Aliases { get; } = Array.Empty<string>();
    public override string Description { get; } = "Reloads Neuron Plugins and Modules";
    public override string Permission { get; } = "neuron.reload";
    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult commandResult)
    {
        try
        {
            Globals.Get<ReloadModule>().Reload();
            commandResult.Response = "Reloaded";
        }
        catch(Exception ex)
        {
            commandResult.Response = "Error while reloading. Check console for further information";
            Logger.Error("Error while reloading\n" + ex);
            commandResult.Success = false;

        }
    }
}
