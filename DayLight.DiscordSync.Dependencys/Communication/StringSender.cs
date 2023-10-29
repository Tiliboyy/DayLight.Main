namespace DayLight.DiscordSync.Dependencys.Communication;

public struct StringSender
{
    public string String { get; set; }
    public StringSender(string text)
    {
        this.String = text;
    }
}
