using Newtonsoft.Json;
using System;

namespace DayLight.DiscordSync.Dependencys.Communication;

[Serializable]
public struct PluginSender
{
    public MessageType Type { get; set; }
    public string Nickname { get; set; }
    public string DataPS { get; set; }
    public double Ping { get; set; }
    
    public T GetData<T>()
    {
        return JsonConvert.DeserializeObject<T>(DataPS);
    }
    public void SetData(object o)
    {
        DataPS = JsonConvert.SerializeObject(o);
    }
    public string Serilize()
    {
        return JsonConvert.SerializeObject(this);
    }

    public PluginSender(MessageType messageType = MessageType.None, object data = null, string nickname = "")
    {
        Type = messageType;
        DataPS = JsonConvert.SerializeObject(data);
        Nickname = nickname;
        Ping = 0;
    }
}


