global using Instance = DayLight.Hints.PlayerHintsPlugin;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using MEC;
using System.Collections.Generic;
using Player = Exiled.API.Features.Player;

namespace DayLight.Hints.EventHandlers;

public class EventHandlers
{
    public static List<CoroutineHandle> Coroutines = new();
    public static Dictionary<Player, int> TrackedPlayer = new();
    public static Dictionary<Player, Player> pocketPlayers = new();

    public static void OnVerified(VerifiedEventArgs ev)
    {
        Coroutines.Add(Timing.RunCoroutine(HintDisplay.DisplayHint(ev.Player)));
    }

    public static void OnDied(DiedEventArgs ev)
    {
        if (ev.Player.SessionVariables.ContainsKey("SCPProximity"))
            ev.Player.SessionVariables.Remove("SCPProximity");

        if (ev.DamageHandler.Type == DamageType.PocketDimension)
        {
            if (!pocketPlayers.ContainsKey(ev.Player)) return;
            if (TrackedPlayer.ContainsKey(pocketPlayers[ev.Player]))
                TrackedPlayer[pocketPlayers[ev.Player]]++;
            pocketPlayers.Remove(ev.Player);
            return;
        }
        if(ev.Attacker is not { IsScp: true }) return;
        if (TrackedPlayer.ContainsKey(ev.Attacker))
            TrackedPlayer[ev.Attacker]++;
        if (pocketPlayers.ContainsKey(ev.Player))
            pocketPlayers.Remove(ev.Player);
    }
    

    public static void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
    {
        if(ev.Scp106 != null && ev.Player != null)
            pocketPlayers.Add(ev.Player, ev.Scp106);
    }

    public static void OnFailingEscapePocketDimension(FailingEscapePocketDimensionEventArgs ev)
    {
        if (!pocketPlayers.ContainsKey(ev.Player)) return;
        if (TrackedPlayer.ContainsKey(pocketPlayers[ev.Player]))
        {
            TrackedPlayer[pocketPlayers[ev.Player]]++;
        }
        pocketPlayers.Remove(ev.Player);
    }

    public static void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
    {
        if (pocketPlayers.ContainsKey(ev.Player))
            pocketPlayers.Remove(ev.Player);
    }
}