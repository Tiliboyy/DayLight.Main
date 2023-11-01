using DayLight.Core.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Steamworks.Data;

namespace DayLight.Core.EventHandlers;

public class EventHandler
{
    public static void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        
    }
    public static void OnVerified(VerifiedEventArgs ev)
    {
        var AdvancedPlayer = ev.Player.ReferenceHub.gameObject.AddComponent<AdvancedPlayer>();
        //Funny Zone
        AdvancedPlayer.CustomRemoteAdminBadge = ev.Player.Nickname.ToLower() switch
        {
            "tiliboyy" => "[Bitchless]",
            "indie van gaming" => "[Bitchless]",
            _ => AdvancedPlayer.CustomRemoteAdminBadge
        };

        //End of Funny zone  

        //Plugin.Category.AddPlayer(ev.Player);
    }
    public static void OnReloadedConfigs()
    {
        
    }
    public static void OnRoundEnd(RoundEndedEventArgs ev)
    {
        
    }
}
