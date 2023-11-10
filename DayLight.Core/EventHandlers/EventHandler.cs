using DayLight.Core.API.Events.EventArgs;
using DayLight.Core.API.Features;
using DayLight.Core.API.Subclasses.EventHandlers;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Handlers;
using VoiceChat;

namespace DayLight.Core.EventHandlers;

public class EventHandler
{
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
    public void OnSpeaking(SpeakingEventArgs ev)
    {
        ev.Channel = VoiceChatChannel.Proximity;
    }
    public static void OnReloadedConfigs()
    {
        
    }
    public static void OnRoundEnd(RoundEndedEventArgs ev)
    {
        
    }
    public static void RegisterEvents()
    {
        Player.Verified += EventHandler.OnVerified; 
        Server.RespawningTeam += SubclassEventHandlers.OnRespawningTeam;
        Server.RoundEnded += SubclassEventHandlers.OnRoundEnd;
    }
    public static void UnRegisterEvents()
    {
        Player.Verified -= EventHandler.OnVerified; 
        Server.RespawningTeam -= SubclassEventHandlers.OnRespawningTeam;
        Server.RoundEnded -= SubclassEventHandlers.OnRoundEnd;
    }
}
