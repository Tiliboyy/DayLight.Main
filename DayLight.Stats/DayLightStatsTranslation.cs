﻿#region

using Neuron.Modules.Configs.Localization;
using System;

#endregion

namespace DayLight.Stats;

[Serializable]

public class DayLightStatsTranslation : Translations<DayLightStatsTranslation>
{
    public string SelfAchiveText { get; set; } =
        "<color=green>Du hast das Achivement %achievement% freigeschalten!</color>";

    public string AllAchiveText { get; set; } =
        "<color=green>%player% hat das Achivement %achievement% freigeschalten!</color>";
}
