using DayLight.DiscordSync.Dependencys.Utils;
using LiteDB;
using System.Collections.Generic;
using System.ComponentModel;

namespace DayLight.DiscordSync.Dependencys.Stats;
using System.ComponentModel;

public interface IDatabasePlayer
{
    
    public bool IsDummy { get; } 
    public string SteamID { get; }
    public string Nickname { get; set; }
    public Stats Stats { get; set; }
    public bool Profileprivate { get; set; }
    public List<Warn> Warns { get; set; }
    
    public event PropertyChangedEventHandler PropertyChanged;
    
    

}
public class DummyDatabasePlayer : IDatabasePlayer
{

    public bool IsDummy { get; } = true;
    public string SteamID { get; } = "Dummy";
    public string Nickname { get; set; } = "Dummy";
    public Stats Stats { get; set; } = null;
    public bool Profileprivate { get; set; } = true;
    public List<Warn> Warns { get; set; } = new List<Warn>();
    public event PropertyChangedEventHandler PropertyChanged;
}

public class DatabasePlayer : INotifyPropertyChanged, IDatabasePlayer
{
    public bool IsDummy { get; } = false;
    private string steamID;
    private string nickname;
    private Stats stats;
    private bool profileprivate;
    private List<Warn> warns;

    public DatabasePlayer(string steam64SteamID, string nickname)
    {
        SteamID = steam64SteamID;
        Nickname = nickname;
        Stats = new Stats();
        Profileprivate = false;
        Warns = new List<Warn>();
    }

    [BsonId]
    public string SteamID
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

    private Dictionary<ItemType, float> usedItems;
    private List<double> unlockedAchievements;

    public Stats()
    {
        UsedItems = new Dictionary<ItemType, float>();
        UnlockedAchievements = new List<double>();
    }

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

    public Dictionary<ItemType, float> UsedItems
    {
        get => usedItems;
        set
        {
            usedItems = value;
            OnPropertyChanged(nameof(UsedItems));
        }
    }

    public List<double> UnlockedAchievements
    {
        get => unlockedAchievements;
        private set
        {
            unlockedAchievements = value;
            OnPropertyChanged(nameof(UnlockedAchievements));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

