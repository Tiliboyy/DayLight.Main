using Exiled.API.Features;
using Neuron.Core.Events;
using VoiceChat;

namespace DayLight.Core.API.Events.EventArgs;

public class SpeakingEventArgs : IEvent
{
    public Player Speaker { get; }
    public Player Target { get; }
    public VoiceChatChannel Channel { get; set; }
    public bool IsAllowed { get; set; }
    
    public SpeakingEventArgs(ReferenceHub player, ReferenceHub target, VoiceChatChannel channel, bool isAllowed = true)
    {
        Speaker = Player.Get(player);
        Target = Player.Get(target);
        IsAllowed = isAllowed;
        Channel = channel;
    }
}
