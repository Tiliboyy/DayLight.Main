#region

using DayLight.Core.API;
using DayLight.Core.API.Attributes;
using Exiled.API.Features;
using Neuron.Core.Plugins;
using Neuron.Modules.Reload;
using System;
using System.IO;

#endregion

namespace DayLight.Moderation;

[Plugin(Name = "ModerationSystem", Author = "Tiliboyy")]
public class ModerationSystemPlugin : DayLightCoreModule<ModerationConfig, ModerationTranslation>
{
    public static ModerationSystemPlugin Instance;
    
    public override void Enabled()
    {
        try
        {
            if (!Directory.Exists(Path.Combine(Paths.Configs, "ModerationSystem/")))
                Directory.CreateDirectory(Path.Combine(Paths.Configs, "ModerationSystem/"));
            Instance = this;
            Core.API.Logger.Warn("subscibing");
            Core.API.Events.Handlers.RemoteAdmin.RequestingPlayerData.Subscribe(EventHandler.OnRequestingData);
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