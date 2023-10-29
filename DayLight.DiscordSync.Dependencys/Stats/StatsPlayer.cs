using DayLight.DiscordSync.Dependencys.Utils;
using System;
using System.Collections.Generic;

namespace DayLight.DiscordSync.Dependencys.Stats;

public class StatsPlayer
{
    public double Kills { get; set; }
    public double KilledScps { get; set; }
    public double PinkCandyKills { get; set; }
    public double Deaths { get; set; }
    public double PlayedRounds { get; set; }
    public double FastestEscapeSeconds { get; set; } 
    
    public Dictionary<ItemType, float> UsedItems { get; set; } = new Dictionary<ItemType, float>();

    public List<double> UnlockedAchivements { get; set; } = new List<double>();
}
