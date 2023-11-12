using DayLight.Dependencys.Utils;
using LiteDB;
using System.Collections.Generic;
using System.ComponentModel;

namespace DayLight.Dependencys.Stats;

public interface IDatabasePlayer
{
    
    public bool IsDummy { get; } 
    [BsonId]
    public ulong SteamID { get; }
    public string Nickname { get; set; }
    public Stats Stats { get; set; }
    public bool Profileprivate { get; set; }
    public List<Warn> Warns { get; set; }
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    

}
public class DummyDatabasePlayer : IDatabasePlayer
{

    public bool IsDummy { get; } = true;
    public ulong SteamID { get; } = 0;
    public string Nickname { get; set; } = "Dummy";
    public Stats Stats { get; set; } = null;
    public bool Profileprivate { get; set; } = true;
    public List<Warn> Warns { get; set; } = new List<Warn>();
    public event PropertyChangedEventHandler PropertyChanged;
}

public class DatabasePlayer : INotifyPropertyChanged, IDatabasePlayer
{
    public bool IsDummy { get; } = false;
    private ulong steamID;
    private string nickname;
    private Stats stats;
    private bool profileprivate;
    private List<Warn> warns;

    public DatabasePlayer(ulong steam64SteamID, string nickname)
    {
        SteamID = steam64SteamID;
        Nickname = nickname;
        Stats = new Stats();
        Profileprivate = false;
        Warns = new List<Warn>();
    }

    [BsonId]
    public ulong SteamID
    {
        get => steamID;
        private set
        {
            steamID = value;
            OnPropertyChanged(nameof(SteamID));
        }
    }

    public string Nickname
    {
        get => nickname;
        set
        {
            nickname = value;
            OnPropertyChanged(nameof(Nickname));
        }
    }

    public Stats Stats
    {
        get => stats;
        set
        {
            stats = value;
            OnPropertyChanged(nameof(Stats));
        }
    }

    public bool Profileprivate
    {
        get => profileprivate;
        set
        { 
            profileprivate = value;
            OnPropertyChanged(nameof(Profileprivate));
        }
    }

    public List<Warn> Warns
    {
        get => warns;
        set
        {
            warns = value;
            OnPropertyChanged(nameof(Warns));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
public class Warns
{
    private List<Warn> warns = new List<Warn>();
    public void AddWarn()
    {
        
    }
}

public class Stats : INotifyPropertyChanged
{
    private double kills;
    private double killedScps;
    private double pinkCandyKills;
    private double deaths;
    private double playedRounds;
    private double fastestEscapeSeconds;
    private float money;

    private CustomDictionary<ItemType, float> usedItems;

    public Stats()
    {
        UsedItems = new CustomDictionary<ItemType, float>();
        UnlockedAchievements = new CustomList<double>();
    }

    public CustomList<double> UnlockedAchievements { get; private set; }

    public double Kills
    {
        get => kills;
        set
        {
            kills = value;
            OnPropertyChanged(nameof(Kills));
        }
    }

    public double KilledScps
    {
        get => killedScps;
        set
        {
            killedScps = value;
            OnPropertyChanged(nameof(KilledScps));
        }
    }

    public double PinkCandyKills
    {
        get => pinkCandyKills;
        set
        {
            pinkCandyKills = value;
            OnPropertyChanged(nameof(PinkCandyKills));
        }
    }

    public double Deaths
    {
        get => deaths;
        set
        {
            deaths = value;
            OnPropertyChanged(nameof(Deaths));
        }
    }

    public double PlayedRounds
    {
        get => playedRounds;
        set
        {
            playedRounds = value;
            OnPropertyChanged(nameof(PlayedRounds));
        }
    }

    public double FastestEscapeSeconds
    {
        get => fastestEscapeSeconds;
        set
        {
            fastestEscapeSeconds = value;
            OnPropertyChanged(nameof(FastestEscapeSeconds));
        }
    }

    public float Money
    {
        get => money;
        set
        {
            money = value;
            OnPropertyChanged(nameof(Money));
        }
    }

    public CustomDictionary<ItemType, float> UsedItems
    {
        get => usedItems;
        set
        {
            usedItems = value;
            OnPropertyChanged(nameof(UsedItems));
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    
}

