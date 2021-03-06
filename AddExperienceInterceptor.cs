using Harmony;
using JetBrains.Annotations;

namespace BetterExperienceSystem
{
    [HarmonyPatch(typeof(KerbalRoster))]
    [HarmonyPatch(nameof(KerbalRoster.AddExperienceType))]
    [UsedImplicitly]
    public static class AddExperienceInterceptor
    {
        [UsedImplicitly]
        private static bool Prefix(string type, string typeName, float notHomeValue, float homeValue)
        {
            BetterExperienceSystem.Instance.RegisterNewExperienceType(type, typeName, notHomeValue, homeValue);
            return true;
        }
    }
}