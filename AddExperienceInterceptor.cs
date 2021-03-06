using Harmony;
using UnityEngine;

namespace BetterExperienceSystem
{
    [HarmonyPatch(typeof(KerbalRoster))]
    [HarmonyPatch(nameof(KerbalRoster.AddExperienceType))]
    public static class AddExperienceInterceptor
    {
        static bool Prefix(string type, string typeName, float notHomeValue, float homeValue)
        {
            BetterExperienceSystem.Instance.RegisterNewExperienceType(type, typeName, notHomeValue, homeValue);
            return true;
        }
    }
}