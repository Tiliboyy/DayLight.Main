using DayLight.Dependencys.Communication.Enums;
using Newtonsoft.Json;
using System;

namespace DayLight.Dependencys.Communication;

[Serializable]
public struct BotRequester
{
    public MessageType Type { get; set; }
    public ulong UserID { get; set; }
    public string DataBR { get; set; }


    public BotRequester(MessageType messageType = MessageType.None, object data = null, ulong userID = 0) 
    {
        Type = messageType;
        UserID = userID;
        try
        {
            DataBR = JsonConvert.SerializeObject(data);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public T GetData<T>()
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(DataBR);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return default;
        }
    }
    public void SetData(object o)
    {
        DataBR = JsonConvert.SerializeObject(o);
    }
    public string Serilize()
    {
        return JsonConvert.SerializeObject(this);
    }
}
