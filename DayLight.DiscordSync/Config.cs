using Exiled.API.Interfaces;
using System.ComponentModel;

namespace DiscordSync.Plugin;

[Serializable]
public class Config : IConfig
{
    public ushort ServerPort { get; set; } = 6969;

    public bool RoleSync { get; set; } = false;
    [Description("Needs RoleSync to be true")]
    public List<PlayTimeRole> Ranks { get; set; } = new()
    {
        new PlayTimeRole { Priority = 1, RankName = "dev", RequiredMinutes = 60, DiscordRankID = 1234 },
        new PlayTimeRole { Priority = 2, RankName = "eclipse", RequiredMinutes = 600, DiscordRankID = 123 }
    };
    public string IpAdress { get; set; } = "127.0.0.1";
    [Description("Enables the Plugin")]
    public bool IsEnabled { get; set; } = true;

    public bool Debug { get; set; } = false;
    public struct PlayTimeRole
    {
        public int Priority { get; set; }
        public float RequiredMinutes { get; set; }
        public string RankName { get; set; }

        public ulong DiscordRankID { get; set; }
    }
}
