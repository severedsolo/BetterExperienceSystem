using Harmony;
using JetBrains.Annotations;

namespace BetterExperienceSystem
{
    [HarmonyPatch(typeof(KerbalRoster))]
    [HarmonyPatch(nameof(KerbalRoster.GetExperienceLevelRequirement))]
    [UsedImplicitly]
    public static class ExperienceLevelRequirementOverride
    {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once RedundantAssignment
        private static void Postfix(int level, ref float __result)
        {
            //Stock calls this as a Zero based index so we have to go +1
            switch (level+1)
            {
                case 2:
                    __result = Settings.Lv1Target;
                    return;
                case 3:
                    __result = Settings.Lv2Target;
                    return;
                case 4:
                    __result = Settings.Lv3Target;
                    return;
                case 5:
                    __result = Settings.Lv4Target;
                    return;
                case 6:
                    __result = Settings.Lv5Target;
                    return;
                default: 
                    __result = 0;
                    return;
            }
        }
    }
}