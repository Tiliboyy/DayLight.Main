using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using DiscordSync.Plugin.Link;
using Exiled.API.Features;
using MEC;
using Neuron.Core.Meta;
using Random = UnityEngine.Random;

namespace DiscordSync.Plugin.Commands.ClientConsole;

[Automatic]
[Command(new [] { Platform.ClientConsole })]
public class LinkCommand : CustomCommand
{

    public override string Command { get; } = "Link";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Links a Player";

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult response)
    {
        if (player.DoNotTrack)
        {
            response.Response = "Du hast DoNotTrack aktiviert du kannst dich nicht linken";
            response.Success = false;
            return;
        }
        if (!ulong.TryParse(player.RawUserId, out var iResult)) return;
        
        if (HasLinkcode(iResult))
        {
            response.Response = "Du hast bereits einen Code aktiv!";
            response.Success = false;
            return;
        }
        if (LinkDatabase.IsSteam64Linked(iResult))
        {
            response.Response = "Du bist bereits verlinkt!";
            response.Success = false;
            return;
        }
        response.Response = GenerateLinkCode(iResult);
    }


    public static string GenerateLinkCode(ulong steam64ID)
    {
        var Code = GenerateCode();
        while (LinkDatabase.TemporaryCodes.Select(x => x.Code).Contains(Code))
            Code = GenerateCode();
        LinkDatabase.TemporaryCodes.Add(new LinkClass(steam64ID, 60, Code));
        return "Dein Code ist " + Code + " er ist für 60 Sekunden aktiv!";

    }

    public static bool HasLinkcode(ulong steamid)
    {
        return LinkDatabase.TemporaryCodes.Count(x => x.Steam64ID == steamid) >= 1;
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
            foreach (var pair in LinkDatabase.TemporaryCodes)
            {
                pair.Time -= 1;
                if (pair.Time == 0) LinkDatabase.TemporaryCodes.Remove(pair);
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
