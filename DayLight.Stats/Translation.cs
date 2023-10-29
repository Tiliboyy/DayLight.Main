#region

using Exiled.API.Interfaces;
using System;

#endregion

namespace DayLight.Stat;

[Serializable]
public class Translation : ITranslation
{
    public string SelfAchiveText { get; set; } =
        "<color=green>Du hast das Achivement %achievement% freigeschalten!</color>";

    public string AllAchiveText { get; set; } =
        "<color=green>%player% hat das Achivement %achievement% freigeschalten!</color>";
}
