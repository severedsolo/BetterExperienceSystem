using Harmony;
using JetBrains.Annotations;

namespace BetterExperienceSystem
{
    [HarmonyPatch(typeof(KerbalRoster))]
    [HarmonyPatch(nameof(KerbalRoster.CalculateExperience))]
    [UsedImplicitly]
    public static class CalculateExperienceOverride
    {
        // ReSharper disable once RedundantAssignment
        // ReSharper disable once InconsistentNaming
        [UsedImplicitly]
        private static void Postfix( ref float __result, params FlightLog[] logs)
        {
            float xp = 0;
            foreach (FlightLog log in logs)
            {
                for (int i = 0; i < log.Count; i++)
                {
                    
                    FlightLog.Entry e = log[i];
                    xp += Utilities.GetXpForType(e.type, e.target);
                }
            }
            __result = xp;
        }
    }
}