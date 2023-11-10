using Exiled.API.Features;
using JetBrains.Annotations;
using PlayerRoles.PlayableScps.Scp173;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DayLight.Core.API.Features;

public class CategoryManager
{
    public static List<CategoryManager> RemoteAdminCategories = new List<CategoryManager>();
    
    public int Id { get; set; }

    public string Name { get; set; }
    public string Color { get; set; }

    public bool RemovePlayersFromDefaultList { get; set; }
    
    public bool AbovePlayers { get; set; }
    public int Priority { get; set; }
    public int Size { get; set; }
    
    [CanBeNull]
    public CategoryManager Get(int id)
    {
        return RemoteAdminCategories.First(x => x.Id == id);
    }

    public List<ReferenceHub> Players = new List<ReferenceHub>();
    public CategoryManager(string name,int id,int priority,bool abovePlayers, bool removePlayersFromDefaultList, Misc.PlayerInfoColorTypes color, int size = 20)
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
    public void AddPlayer(List<Player> players)
    {
        players.ForEach(x=> Players.Add(x.ReferenceHub));
    }
    public void AddPlayer(Player player)
    {
        Players.Add(player.ReferenceHub);
    }

}