using Newtonsoft.Json;
using System;

namespace DayLight.DiscordSync.Dependencys.Communication;

[Serializable]
public struct PluginSender
{
    public DataType Type { get; set; }
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

    public PluginSender(DataType dataType = DataType.None, object data = null, string nickname = "")
    {
        this.Type = dataType;
        this.DataPS = JsonConvert.SerializeObject(data);
        this.Nickname = nickname;
        this.Ping = 0;

    }
}


