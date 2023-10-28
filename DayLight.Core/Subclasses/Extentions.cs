using DayLight.Core.Subclasses.Features;
using Exiled.API.Features;

namespace DayLight.Core.Subclasses;

public static class Extentions
{
    public static Subclass GetSubclass(this Player player)
    {
        return Subclass.GetSubclass(player); 
    }
}
