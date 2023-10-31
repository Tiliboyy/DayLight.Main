using CommandSystem;
using DiscordSync.Plugin.Link;
using Exiled.API.Features;
using MEC;
using Random = UnityEngine.Random;

namespace DiscordSync.Plugin.Commands.ClientConsole;

[CommandHandler(typeof(ClientCommandHandler))]
[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class LinkCommand : ICommand
{

    public string Command { get; } = "Link";

    public string[] Aliases { get; } = Array.Empty<string>();

    public string Description { get; } = "Links a Player";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender);
        if (player.DoNotTrack)
        {
            response = "Du hast DoNotTrack aktiviert du kannst dich nicht linken";
            return true;
        }
        if (ulong.TryParse(player.RawUserId, out var iResult))
        {
            if (HasLinkcode(iResult))
            {
                response = "Du hast bereits einen Code aktiv!";
                return true;
            }
            if (LinkDatabase.IsSteam64Linked(iResult))
            {
                response = "Du bist bereits verlinkt!";
                return true;
            }
            response = GenerateLinkCode(iResult);
            return true;
        }

        response = "response";
        return true;
    }
    public static string GenerateLinkCode(ulong steam64ID)
    {
        var Code = GenerateCode();
        while (LinkDatabase.OpenLinks.Select(x => x.Code).Contains(Code))
            Code = GenerateCode();
        LinkDatabase.OpenLinks.Add(new LinkClass(steam64ID, 60, Code));
        return "Dein Code ist " + Code + " er ist für 60 Sekunden aktiv!";

    }

    public static bool HasLinkcode(ulong steamid)
    {
        return LinkDatabase.OpenLinks.Count(x => x.Steam64ID == steamid) >= 1;
    }

    public static int GenerateCode()
    {
        var code = Random.Range(1000, 9999);
        return code;
    }
    public static IEnumerator<float> Timer()
    {
        for (;;)
        {
            yield return Timing.WaitForSeconds(1f);
            foreach (var pair in LinkDatabase.OpenLinks)
            {
                pair.Time -= 1;
                if (pair.Time == 0) LinkDatabase.OpenLinks.Remove(pair);
            }
        }
    }
    public class LinkClass
    {
        public LinkClass(ulong steam64ID, float time, int code)
        {
            Steam64ID = steam64ID;
            Time = time;
            Code = code;
        }

        public ulong Steam64ID { get; set; }

        public int Code { get; set; }
        public float Time { get; set; }
    }
}
