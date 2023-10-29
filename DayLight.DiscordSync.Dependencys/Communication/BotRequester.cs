using Newtonsoft.Json;
using System;

namespace DayLight.DiscordSync.Dependencys.Communication;

[Serializable]
public struct BotRequester
{
    public DataType Type { get; set; }
    public ulong UserID { get; set; }
    public string DataBR { get; set; }


    public BotRequester(DataType dataType = DataType.None, object data = null, ulong userID = 0) 
    {
        Type = dataType;
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
