namespace DayLight.Dependencys.Models.Helpers;

public class PlayerInformationHelper
{
    public DatabasePlayer DatabaseEntry{ get; set; }
    public double PlaytimeSeconds { get; set; }

    public PlayerInformationHelper(DatabasePlayer databaseEntry, double playtime)
    {
        this.DatabaseEntry = databaseEntry;
        this.PlaytimeSeconds = playtime;
    }
}
