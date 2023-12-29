﻿using DayLight.Core.API;
using DayLight.Misc.Configs;
using Exiled.Events.Handlers;
using HarmonyLib;
using Neuron.Core.Plugins;

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