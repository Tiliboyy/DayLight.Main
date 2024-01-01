#region

using DayLight.Core.API;
using DayLight.Moderation.Configs;
using Exiled.API.Features;
using Neuron.Core.Plugins;
using System;
using System.IO;

#endregion

namespace DayLight.Moderation;

[Plugin(Name = "ModerationSystem", Author = "Tiliboyy")]
public class ModerationSystemPlugin : DayLightCorePlugin<ModerationConfig, ModerationTranslation>
{
    public static ModerationSystemPlugin Instance;

    protected override void Enabled()
    {
        try
        {
            if (!Directory.Exists(Path.Combine(Paths.Configs, "ModerationSystem/")))
                Directory.CreateDirectory(Path.Combine(Paths.Configs, "ModerationSystem/"));
            Instance = this;
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