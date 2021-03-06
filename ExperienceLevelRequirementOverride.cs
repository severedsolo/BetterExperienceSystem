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
        private static void Postfix(int level, ref float result)
        {
            //Stock calls this as a Zero based index so we have to go +1
            switch (level+1)
            {
                case 2:
                    result = Settings.Lv1Target;
                    return;
                case 3:
                    result = Settings.Lv2Target;
                    return;
                case 4:
                    result = Settings.Lv3Target;
                    return;
                case 5:
                    result = Settings.Lv4Target;
                    return;
                case 6:
                    result = Settings.Lv5Target;
                    return;
                default: 
                    result = 0;
                    return;
            }
        }
    }
}