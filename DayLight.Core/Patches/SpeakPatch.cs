using DayLight.Core.API.Events.EventArgs;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using HarmonyLib;
using JetBrains.Annotations;
using Mirror;
using Neuron.Core.Meta;
using PlayerRoles.Voice;
using System;
using VoiceChat;
using VoiceChat.Networking;

namespace DayLight.Core.Patches;


[Automatic]
[HarmonyPatch]
public static class SpeakPatch
{    

    [UsedImplicitly]
    [HarmonyPrefix]
    [HarmonyPatch(typeof(VoiceTransceiver), nameof(VoiceTransceiver.ServerReceiveMessage))]
    public static bool OnServerReceiveMessage(NetworkConnection conn, VoiceMessage msg)
    {
        try
        {
            if (msg.SpeakerNull || (int) msg.Speaker.netId != (int) conn.identity.netId || msg.Speaker.roleManager.CurrentRole is not IVoiceRole currentRole1 
                || !currentRole1.VoiceModule.CheckRateLimit() || VoiceChatMutes.IsMuted(msg.Speaker))
                return false;
            var channel = currentRole1.VoiceModule.ValidateSend(msg.Channel);
            if (channel == VoiceChatChannel.None)
                return false;
            currentRole1.VoiceModule.CurrentChannel = channel;
            foreach (var hub in ReferenceHub.AllHubs)
            {
                if (hub.roleManager.CurrentRole is not IVoiceRole currentRole2) continue;

                var args = new SpeakingEventArgs(msg.Speaker, hub, channel);
                API.Events.Handlers.VoiceChat.Speaking.Raise(args);
                if (!args.IsAllowed) return false;
                var voiceChatChannel = currentRole2.VoiceModule.ValidateReceive(args.Speaker.ReferenceHub, args.Channel);
                if (voiceChatChannel == VoiceChatChannel.None) continue;
                msg.Channel = voiceChatChannel;
                hub.connectionToClient.Send(msg);
            }

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
