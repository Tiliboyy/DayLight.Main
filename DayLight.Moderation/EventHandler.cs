#region

#endregion

namespace DayLight.Moderation;

public class EventHandler
{

    /*
    public static void OnRequestingData(RequestingPlayerDataEventArgs ev)
    {
        Logger.Info("executed");
        var player = ev.Player;
        if (player == null) return;
        
        //Funny Zone
        var stringBuilder = new StringBuilder();
        switch (player.Nickname.ToLower())
        {
            case "tiliboyy":
                stringBuilder.Append($"\n<b><color=#f102f9>WENN DU MIR PUNKTE GIBST REISS ICH DEINE BALLS AUS</color></b>");
                break;
            case "indie van gaming":
                stringBuilder.Append($"\nFreunde:<color=#fca505> Fortnite</color>");
                break;
            case "peter":
                stringBuilder.Append($"\nStatus: <color=#fc05e3>Tinybrain</color>");
                break;
            case "schwert300":
                stringBuilder.Append($"<color=#ed1515>RDMer</color>");
                break;
            case "artixthewolf":
                stringBuilder.Append($"<color=#cc00cc>sexo</color>");
                break;
            case "fes":
                stringBuilder.Append($"<color=#ffff00>Finanzen Eingesammelt: 42069€</color>");
                break;
            case "leotz":
                stringBuilder.Append($"<color=#ff80df>Cry about it</color>");
                break;


        }

        var total = WarnDatabase.Database.GetTotal(player.UserId);
        stringBuilder.Append($"\nPunkte: <color={GetColor(total)}>{total}</color>");
        var databasePlayer = SCPUtils.DatabasePlayer.GetDatabasePlayer(player);
        if (databasePlayer != null)
        {
            if (player.DoNotTrack)
            {
                stringBuilder.Append($"\nPlaytime: Do Not Track");
            }
            else
            {
                stringBuilder.Append($"\nPlaytime: {new TimeSpan(0, 0, databasePlayer.PlayTimeRecords.Values.Sum()).ToString()}");
            }
        }
        else
        {
            stringBuilder.Append(player.DoNotTrack ? $"\nPlaytime: Do Not Track" : $"\nPlaytime: NULL");

        }
        ev.Message += $"\n{stringBuilder}";
    }
    */
    private static string GetColor(float value)
    {
        return value switch
        {
            >= 4 => "red",
            0 => "green",
            > 0 and < 4 => "yellow",
            _ => "green"
        };
    }
}
