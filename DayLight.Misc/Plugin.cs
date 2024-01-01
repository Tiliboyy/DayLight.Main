using DayLight.Core.API;
using DayLight.Misc.Configs;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using Neuron.Core.Plugins;
using PlayerRoles;
using System.Collections.Generic;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace DayLight.Misc;

[Plugin(Name = "DayLight.Misc", Author = "Tiliboyy")]
public class MiscPlugin : DayLightCorePlugin<MiscConfig, MiscTranslation>
{
    public static MiscPlugin Instance;
    public static bool EnableTeamRespawns = true;

    protected override void Enabled()
    {
        Instance = this;
        Player.Hurting += Eventhandlers.OnHurt;
        Player.UsedItem += Eventhandlers.OnUsingItem;
        Server.RoundEnded += Eventhandlers.OnRoundEnd;
    }
}