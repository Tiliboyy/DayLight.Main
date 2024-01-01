using DayLight.Core.API;
using DayLight.Core.API.Database;

namespace DayLight.Sync.Link;

public class LinkUtils
{
    public static bool IsSteam64Linked(ulong steam64id)
    {
        var dbPlayer = DayLightDatabase.GetDatabasePlayer(steam64id);
        return dbPlayer != null && dbPlayer.DiscordID != 0;
    }
    public static bool IsUserIdLinked(ulong UserId)
    {
        var dbPlayer = DayLightDatabase.GetDatabasePlayerDiscord(UserId);
        return dbPlayer != null && dbPlayer.DiscordID != 0;
    }
    public static ulong GetLinkedUserID(ulong Steam64Id)
    {
        var dbPlayer = DayLightDatabase.GetDatabasePlayer(Steam64Id);
        return dbPlayer?.DiscordID ?? 0;
    }
    public static ulong GetLinkedSteamID(ulong DiscordID)
    {
        var dbPlayer = DayLightDatabase.GetDatabasePlayerDiscord(DiscordID);
        return dbPlayer?.SteamID ?? 0;
    }
    public static bool Unlink(ulong Steam64ID)
    {
        var dbPlayer = DayLightDatabase.GetDatabasePlayer(Steam64ID);
        if (dbPlayer != null) 
            dbPlayer.DiscordID = 0;
        else 
            return false;
        return true;
    }
    public static bool Link(ulong UserID, int Code)
    {
        var Codes = TemporaryCodes.Where(linkClass => linkClass.Code == Code).ToList();
        if (Codes.Count == 0) return false;
        foreach (var linkClass in Codes.Where(linkClass => TemporaryCodes.Contains(linkClass)))
        {
            TemporaryCodes.Remove(linkClass);
        }
        var flag = UpdateLink(Codes.First().Steam64ID, UserID);
        return flag;

    }
    public static bool UpdateLink(ulong Steam64ID, ulong UserID)
    {
       var dbPlayer =  DayLightDatabase.GetDatabasePlayer(Steam64ID);
       if (dbPlayer != null)
       {
           dbPlayer.DiscordID = UserID;
           Logger.Debug($"Set link to {dbPlayer.Nickname}");
           DayLightDatabase.UpdatePlayer(dbPlayer);

       }
       else
       {
           return false;
       }
       return true;
    }

    public static List<Commands.ClientConsole.Link.LinkClass> TemporaryCodes = new();
}