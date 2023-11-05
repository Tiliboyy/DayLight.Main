using DayLight.Core.API.Events.EventArgs;
using Neuron.Core.Events;

namespace DayLight.Core.API.Events.Handlers;

public class VoiceChat
{
    public static EventReactor<SpeakingEventArgs> Speaking = new();
}
