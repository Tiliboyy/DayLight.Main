using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;

//using FuckExiled.API.Features;

namespace DayLight.Moderation.Report;

public class ReportPlayerList
{
    public static void Load()
    {
        //CategoryManager.RemoteAdminCategory.Create("<b>[Report]</b>", Misc.PlayerInfoColorTypes.Red);
        Server.LocalReporting += OnReported;
    }
    public static void OnReported(LocalReportingEventArgs ev)
    {
        //AdvancedPlayer.Get(ev.Player)!.CustomRemoteAdminBadge = "[Reporter]";
        //AdvancedPlayer.Get(ev.Target)!.CustomRemoteAdminBadge = "[Reported]";
    }
}
