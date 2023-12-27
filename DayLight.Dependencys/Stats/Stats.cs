using DayLight.Dependencys.Utils;
using LiteDB;
using System.Collections.Generic;
using System.ComponentModel;

namespace DayLight.Dependencys.Stats;

public interface IDatabasePlayer
{

    public bool IsDummy { get; set; }
    [BsonId]
    public ulong SteamID { get; }
    public string Nickname { get; set; }
    public Stats Stats { get; set; }
    public List<Warn> Warns { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

}

public class DatabasePlayer : INotifyPropertyChanged, IDatabasePlayer
{
    public bool IsDummy { get; set; }= false;
    private ulong steamID;
    private ulong discordID;
    private string nickname;
    private Stats stats;
    private bool profileprivate;
    private List<Warn> warns;

    public DatabasePlayer()
    {
        IsDummy = true;
        steamID = 99999999;
    }
    public DatabasePlayer(ulong steam64SteamID, string nickname)
    {
        SteamID = steam64SteamID;
        Nickname = nickname;
        Stats = new Stats();
        Public = false;
        DiscordID = 0;
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
    public ulong DiscordID
    {
        get => discordID;
        set
        {
            discordID = value;
            OnPropertyChanged(nameof(DiscordID));
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

    public bool Public
    {
        get => profileprivate;
        set
        { 
            profileprivate = value;
            OnPropertyChanged(nameof(Public));
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
    private double _kills;
    private double _killedScps;
    private double _pinkCandyKills;
    private double _deaths;
    private double _playedRounds;
    private double _fastestEscapeSeconds;
    private float _money;
    private bool _public;
    private CustomDictionary<ItemType, float> _usedItems;

    public Stats()
    {
        UsedItems = new CustomDictionary<ItemType, float>();
        UnlockedAchievements = new CustomList<double>();
    }

    public bool Public
    {
        get => _public;
        set
        {
            _public = value;
            OnPropertyChanged(nameof(_public));
        }
    }

    public CustomList<double> UnlockedAchievements { get; private set; }

    public double Kills
    {
        get => _kills;
        set
        {
            _kills = value;
            OnPropertyChanged(nameof(Kills));
        }
    }

    public double KilledScps
    {
        get => _killedScps;
        set
        {
            _killedScps = value;
            OnPropertyChanged(nameof(KilledScps));
        }
    }

    public double PinkCandyKills
    {
        get => _pinkCandyKills;
        set
        {
            _pinkCandyKills = value;
            OnPropertyChanged(nameof(PinkCandyKills));
        }
    }

    public double Deaths
    {
        get => _deaths;
        set
        {
            _deaths = value;
            OnPropertyChanged(nameof(Deaths));
        }
    }

    public double PlayedRounds
    {
        get => _playedRounds;
        set
        {
            _playedRounds = value;
            OnPropertyChanged(nameof(PlayedRounds));
        }
    }

    public double FastestEscapeSeconds
    {
        get => _fastestEscapeSeconds;
        set
        {
            _fastestEscapeSeconds = value;
            OnPropertyChanged(nameof(FastestEscapeSeconds));
        }
    }

    public float Money
    {
        get => _money;
        set
        {
            _money = value;
            OnPropertyChanged(nameof(Money));
        }
    }

    public CustomDictionary<ItemType, float> UsedItems
    {
        get => _usedItems;
        set
        {
            _usedItems = value;
            OnPropertyChanged(nameof(UsedItems));
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    
}

