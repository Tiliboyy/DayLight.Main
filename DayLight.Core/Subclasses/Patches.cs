using Exiled.API.Features;
using HarmonyLib;
using Neuron.Core.Meta;
using PlayerRoles.RoleAssign;
using System;

namespace DayLight.Core.Subclasses;



[Automatic]
[HarmonyPatch]
public class Patches
{
    [HarmonyPatch(typeof(RoleAssigner), nameof(RoleAssigner.OnRoundStarted))]
    public static class RoundStartPostfix
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(RoleAssigner), nameof(RoleAssigner.OnRoundStarted))]
        public static void RoundStartedPostfix()
        {
            try
            {
                EventHandlers.SubclassEventHandlers.RoundStarted();
            }
            catch (Exception e)
            {
                Logger.Error("RoundStart Postfix Failed: " + e);
                throw;
            }
        }
    }
}