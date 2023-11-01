using DayLight.DiscordSync.Dependencys.Utils;
using System;
using System.Collections.Generic;

namespace DayLight.DiscordSync.Dependencys.Stats;
public class DatabasePlayer
{
    public DatabasePlayer(string Steam64ID, string nickname)
    {
        _id = Steam64ID;
        Nickname = nickname;
        Stats = new Stats();
        Private = false;
        Warns = new List<Warn>();
    }
    public string _id { get; private set; }

    public string Nickname { get; set; }

    public Stats Stats { get; private set; }
    public bool Private { get; set; }
    
    public List<Warn> Warns { get; set; }


}
public class Stats
{
    public double Kills { get; set; }
    public double KilledScps { get; set; }
    public double PinkCandyKills { get; set; }
    public double Deaths { get; set; }
    public double PlayedRounds { get; set; }
    public double FastestEscapeSeconds { get; set; } 
    
    public float Money { get; set; }

    
    public Dictionary<ItemType, float> UsedItems { get; set; } = new();

    public List<double> UnlockedAchivements { get; set; } = new();
}


