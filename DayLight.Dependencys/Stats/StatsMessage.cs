namespace DayLight.Dependencys.Stats;

public class PlayerInformation
{
    public IDatabasePlayer DatabasePlayer{ get; set; }
    public double PlaytimeSeconds { get; set; }

    public PlayerInformation(IDatabasePlayer databasePlayer, double playtime)
    {
        this.DatabasePlayer = databasePlayer;
        this.PlaytimeSeconds = playtime;
    }
}
