using DayLight.Core.API.Subclasses.Features;
using Exiled.API.Features;

namespace DayLight.Core.API.Subclasses;

public static class Extentions
{
    public static Subclass GetSubclass(this Player player)
    {
        return Subclass.GetSubclass(player); 
    }
}
