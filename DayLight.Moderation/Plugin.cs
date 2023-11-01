#region

using DayLight.Moderation.Warn;
using Exiled.API.Features;
using Neuron.Core;
using Neuron.Core.Plugins;
using Neuron.Modules.Reload;
using System;
using System.IO;

#endregion

namespace DayLight.Moderation;

[Plugin(Name = "ModerationSystem", Author = "Tiliboyy")]
public class ModerationSystemPlugin : ReloadablePlugin<ModerationConfig, ModerationTranslation>
{
    public static ModerationSystemPlugin Instance;
    
    public override void EnablePlugin()
    {
        try
        {
            if (!Directory.Exists(Path.Combine(Paths.Configs, "ModerationSystem/")))
                Directory.CreateDirectory(Path.Combine(Paths.Configs, "ModerationSystem/"));
            Instance = this;
            //FuckExiled.Events.Handlers.RemoteAdmin.RequestingPlayerData.Subscribe(EventHandler.OnRequestingData);
        }
        catch (Exception error)
        {
            Logger.Error(error);
        }
    }

    public override void Disable()
    {
        Instance = null!;
    }

}