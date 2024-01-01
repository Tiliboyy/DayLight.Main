using Exiled.API.Features;
using Exiled.API.Features.Roles;
using HarmonyLib;
using Mirror;
using Neuron.Core.Meta;
using PlayerRoles;
using PlayerRoles.Voice;
using System;
using VoiceChat;
using VoiceChat.Networking;

namespace DayLight.Core.Patches;


[Automatic]
[HarmonyPatch]
public static class SpeakPatch
{    

    [HarmonyPrefix]
    [HarmonyPatch(typeof(VoiceTransceiver), nameof(VoiceTransceiver.ServerReceiveMessage))]
    public static bool OnServerReceiveMessage(NetworkConnection conn, VoiceMessage msg)
    {
        try
        {

            if (msg.SpeakerNull || 
                (int) msg.Speaker.netId != (int) conn.identity.netId ||
                !(msg.Speaker.roleManager.CurrentRole is IVoiceRole currentRole1) ||
                !currentRole1.VoiceModule.CheckRateLimit() || 
                VoiceChatMutes.IsMuted(msg.Speaker))
                return false;
            var player = Player.Get(msg.Speaker);
            var channel = currentRole1.VoiceModule.ValidateSend(msg.Channel);
            if (channel == VoiceChatChannel.None)
                return false;
            currentRole1.VoiceModule.CurrentChannel = channel;
            var checkForScpProximity = player.Role.Team is Team.SCPs or Team.Flamingos &&
                                       player.SessionVariables.ContainsKey("SCPProximity");
            foreach (var receiver in Player.List)
            {
                if(receiver == player) continue;
                if (receiver.Role.Base is not IVoiceRole voiceRole) continue;
                if (player.SessionVariables.ContainsKey("SCPProximity") &&
                    channel is VoiceChatChannel.ScpChat or VoiceChatChannel.Scp1507)
                    channel = VoiceChatChannel.None;
                var isSpectator = receiver.Role.Type is RoleTypeId.Spectator or RoleTypeId.Overwatch;

                if (checkForScpProximity)
                {
                    if (isSpectator)
                    {
                        var spectating = receiver.CurrentlySpectating();
                        if (spectating == player)
                            channel = VoiceChatChannel.Proximity;
                        else if (DayLightCore.Instance != null && spectating != null &&
                                 (spectating.Position - player.Position).sqrMagnitude < 
                                 DayLightCore.Instance.Config.ProximityRange)
                            channel = VoiceChatChannel.Proximity;
                    }
                    else if (DayLightCore.Instance != null && 
                             (receiver.Position - player.Position).sqrMagnitude < 
                             DayLightCore.Instance.Config.ProximityRange)
                    {
                        channel = VoiceChatChannel.Proximity;
                    }

                }
                else if (DayLightCore.Instance != null &&
                         DayLightCore.Instance.Config.SpectatorChat &&
                         isSpectator &&
                         player.Role.Team is Team.SCPs or Team.Flamingos)
                {
                    if (receiver.CurrentlySpectating()?.Role.Team is Team.SCPs or Team.Flamingos)
                        channel = VoiceChatChannel.Proximity;
                }

                
                var voiceChatChannel = voiceRole.VoiceModule.ValidateReceive(msg.Speaker, channel);
                if (voiceChatChannel == VoiceChatChannel.None) continue;
                msg.Channel = voiceChatChannel;
                Log.Info($"{player.Nickname} is taking to {receiver.Nickname} in {voiceChatChannel}");
                receiver.ReferenceHub.connectionToClient.Send(msg);
            }
            return false;
        }
        catch (Exception ex)
        {
            Log.Error("Speak Patch Failed:\n" + ex);
        }
        return false;
    }
    public static Player CurrentlySpectating(this Player player)
    {
        if (player.Role is SpectatorRole spectatorRole)
            return spectatorRole.SpectatedPlayer;
        return null;
    }


}
