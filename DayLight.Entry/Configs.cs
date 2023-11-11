using Exiled.API.Interfaces;

namespace DayLight.Entry;

public class Configs : IConfig
{

    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; } = false;
}
