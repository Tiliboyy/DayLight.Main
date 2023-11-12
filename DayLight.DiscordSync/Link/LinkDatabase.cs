using DayLight.Dependencys;
using DiscordSync.Plugin.Commands.ClientConsole;
using Exiled.API.Features;
using Utils.NonAllocLINQ;
using YamlDotNet.Serialization;

namespace DiscordSync.Plugin.Link;

public class LinkDatabase
{
    public static List<LinkedPlayer> LinkedPlayers = new();
    public static bool IsSteam64Linked(ulong steam64id)
    {
        ReadYaml();
        var linkedlayers = LinkedPlayers.Where(x => x.Steam64Id == steam64id).ToList();
        return linkedlayers.Count != 0;
    }
    public static bool IsUserIdLinked(ulong UserId)
    {
        ReadYaml();
        var linkedlayers = LinkedPlayers.Where(x => x.UserID == UserId).ToList();
        return linkedlayers.Count != 0;
    }
    public static ulong GetLinkedUserID(ulong Steam64Id)
    {
        ReadYaml();
        var linkedlayers = LinkedPlayers.Where(x => x.Steam64Id == Steam64Id).ToList();
        return linkedlayers.Count == 0 ? 0 : linkedlayers.FirstOrDefault().UserID;
    }
    public static ulong GetLinkedSteamID(ulong DiscordID)
    {
        ReadYaml();      
        var linkedlayers = LinkedPlayers.Where(x => x.UserID == DiscordID).ToList();    
        return linkedlayers.Count == 0 ? 100000000 : linkedlayers.First().Steam64Id; 

    }
    public static bool Unlink(ulong PlayerUserID)
    {
        ReadYaml();
        var links = LinkedPlayers.Where(x => x.Steam64Id == PlayerUserID).ToList();
        if (links.Count == 0)
        {
            return true;
        }

        foreach (var linkedPlayer in links)
        {
            LinkedPlayers.Remove(linkedPlayer);
        }
        UpdateYaml();
        return true;
    }
    public static bool Link(ulong UserID, int Code)
    {

        ReadYaml();
        var linkClasses = OpenLinks.Where(linkClass => linkClass.Code == Code).ToList();
        if (linkClasses.Count == 0) return false;
        foreach (var linkClass in linkClasses.Where(linkClass => OpenLinks.Contains(linkClass)))
        {
            OpenLinks.Remove(linkClass);
        }
        var flag = UpdateLink(linkClasses.First().Steam64ID, UserID);
        UpdateYaml();

        return flag;

    }
    public static bool UpdateLink(ulong Steam64ID, ulong UserID)
    {
        ReadYaml();
        if (LinkedPlayers.Count(x => x.UserID == UserID) != 0) return false;
        LinkedPlayers.Add(new LinkedPlayer
            { Steam64Id = Steam64ID, UserID = UserID });
        UpdateYaml();
        return true;
    }
    public static void UpdateYaml()
    {
        var yaml = new Serializer().Serialize(LinkedPlayers);
        var filePath = Path.Combine(Paths.Configs, "DiscordSync/LinkedPlayers.yaml");
        var backupFilePath = Path.Combine(Paths.Configs, "DiscordSync/LinkedPlayers_backup.yaml");

        try
        {
            File.WriteAllText(filePath, yaml);
        }
        catch (Exception ex)
        {
            Log.Error($"Error writing to LinkedPlayers.yaml: {ex}");
            return;
        }

        try
        {
            File.Copy(filePath, backupFilePath, true);
        }
        catch (Exception ex)
        {
            Log.Error($"Error creating backup file for LinkedPlayers.yaml: {ex}");
        }

        try
        {
            var yamldata = File.ReadAllText(filePath);
            var e = new Deserializer().Deserialize<List<LinkedPlayer>>(new StringReader(yamldata));
            LinkedPlayers = e;
        }
        catch (Exception ex)
        {
            Log.Error($"Error reading from LinkedPlayers.yaml: {ex}");
        }
    }
    public static void ReadYaml()
    {
        if (!File.Exists(Path.Combine(Paths.Configs, "DiscordSync/LinkedPlayers.yaml")))
        {
            UpdateYaml();
        }

        var yamldata = File.ReadAllText(Path.Combine(Paths.Configs, "DiscordSync/LinkedPlayers.yaml"));

        var e = new Deserializer().Deserialize<List<LinkedPlayer>>(new StringReader(yamldata));
        LinkedPlayers = e;
        if (LinkedPlayers.Count == 0)
        {
            LinkedPlayers.Add(new LinkedPlayer
                { Steam64Id = 0, UserID = 0 });
        }
    }

    public static List<LinkCommand.LinkClass> OpenLinks = new();
}