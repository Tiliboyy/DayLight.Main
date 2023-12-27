using DayLight.Core.API;
using DayLight.Core.API.Events.EventArgs;
using DayLight.Core.API.Features;
using DayLight.Core.API.Subclasses.EventHandlers;
using Discord;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Handlers;
using System;
using VoiceChat;

namespace DayLight.Core.EventHandlers;

public class EventHandler
{
    public CustomRaCategory CustomRaCategory = new("Sex", 696969, 5, true, true, Misc.PlayerInfoColorTypes.Aqua);
    public static void OnVerified(VerifiedEventArgs ev)
    {
        var AdvancedPlayer = ev.Player.ReferenceHub.gameObject.AddComponent<AdvancedPlayer>();
        //Funny Zone
        AdvancedPlayer.CustomRemoteAdminBadge = ev.Player.Nickname.ToLower() switch
        {
            "tiliboyy" => "[Debug]",
            "fw_blu" => "[Retard]",
            _ => AdvancedPlayer.CustomRemoteAdminBadge
        };
        //End of Funny zone
    }
    public static void OnReloadedConfigs()
    {
        
    }
    public static void OnRoundEnd(RoundEndedEventArgs ev)
    {
        
    }
    public static void RegisterEvents()
    {
        Player.Verified += OnVerified; 
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
