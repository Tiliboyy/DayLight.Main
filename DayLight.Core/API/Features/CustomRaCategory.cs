using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;

namespace DayLight.Core.API.Features;

public class CustomRaCategory
{
    public static List<CustomRaCategory> RemoteAdminCategories = new();
    
    public int Id { get; set; }

    public string Name { get; set; }
    public string Color { get; set; }

    public bool RemovePlayersFromDefaultList { get; set; }
    
    public bool AbovePlayers { get; set; }
    public int Priority { get; set; }
    public int Size { get; set; }
    
    [CanBeNull]
    public CustomRaCategory Get(int id)
    {
        return RemoteAdminCategories.First(x => x.Id == id);
    }

    public List<ReferenceHub> Players = new();
    public CustomRaCategory(string name,int id,int priority,bool abovePlayers, bool removePlayersFromDefaultList, Misc.PlayerInfoColorTypes color, int size = 20)
    {
        if(RemoteAdminCategories.Exists(x => RemoteAdminCategories.Contains(x)))
            return;
        Id = id;
        Size = size;
        Color = StaticUtils.Colors[color];
        RemovePlayersFromDefaultList = removePlayersFromDefaultList;
        AbovePlayers = abovePlayers;
        Name = name;
        Priority = priority;
        RemoteAdminCategories.Add(this);
    }

}