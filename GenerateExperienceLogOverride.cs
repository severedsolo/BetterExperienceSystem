using System;
using System.Collections.Generic;
using System.Text;
using Harmony;
using UniLinq;

namespace BetterExperienceSystem
{
    [HarmonyPatch(typeof(KerbalRoster))]
    [HarmonyPatch(nameof(KerbalRoster.GenerateExperienceLog))]
    public static class GenerateExperienceLogOverride
    {
        static void Postfix(ref string __result, FlightLog log)
        {
            Dictionary<string, float> xpStrings = new Dictionary<string, float>();
            for (int i = 0; i < log.Entries.Count; i++)
            {
                FlightLog.Entry e = log.Entries.ElementAt(i);
                if (!BetterExperienceSystem.Instance.xpTypes.TryGetValue(e.type, out BetterExperienceType bet)) continue;
                string s = bet.xpTypeName + e.target;
                float additionalXp = Utilities.GetXpForType(e.type, e.target);
                if (!xpStrings.TryGetValue(s, out float newXp)) xpStrings.Add(s, additionalXp);
                else xpStrings[s] += additionalXp;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < xpStrings.Count; i++)
            {
                KeyValuePair<string, float> kvp = xpStrings.ElementAt(i);
                if (Math.Round(kvp.Value, 0) == 0) continue;
                sb.AppendLine(kvp.Key+" = "+Math.Round(kvp.Value, 0));
            }

            __result = sb.ToString();
        }
    }
}