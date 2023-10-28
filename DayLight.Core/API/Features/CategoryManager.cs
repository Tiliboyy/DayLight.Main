using Exiled.API.Features;
using System;
using System.Collections.Generic;

namespace DayLight.Core.API.Features;

public class CategoryManager
{

    public static string GetColorHexCode(string color) => !Enum.TryParse(color, true, out Misc.PlayerInfoColorTypes colorEnum)
        ? Colors[Misc.PlayerInfoColorTypes.White]
        : Colors[colorEnum];
    
    public static Dictionary<Misc.PlayerInfoColorTypes, string> Colors { get; } = Misc.AllowedColors;

    public class RemoteAdminCategory
    {
        public static List<RemoteAdminCategory> RemoteAdminCategories = new List<RemoteAdminCategory>();
        public string Name { get; set; }
        public string Color { get; set; }
        public bool DisplayOnTop { get; set; }
        public int Id { get; set; }
        public int Size { get; set; }
        private int curid { get; set; } = int.MaxValue;

        public List<Player> Players = new List<Player>();
        private RemoteAdminCategory(string name, Misc.PlayerInfoColorTypes color, int size )
        {
            curid--;
            Id = curid;
            Size = size;
            Color = Colors[color];
            Name = name;
            RemoteAdminCategories.Add(this);
        }


        public static RemoteAdminCategory Create(string name, Misc.PlayerInfoColorTypes color = Misc.PlayerInfoColorTypes.White, int size = 20)
        {
            return new RemoteAdminCategory(name, color, size);
        }
        public void AddPlayers(List<Player> players)
        {
            players.ForEach(x=> Players.Add(x));
        }
        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }
    }
}