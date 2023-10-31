namespace DayLight.DiscordSync.Dependencys.Communication;

public struct StringMessage
{
    public string String { get; set; }
    public StringMessage(string text)
    {
        String = text;
    }
}
