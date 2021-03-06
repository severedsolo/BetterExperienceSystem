using Harmony;

namespace BetterExperienceSystem
{
    [HarmonyPatch(typeof(KerbalRoster))]
    [HarmonyPatch(nameof(KerbalRoster.GetExperienceLevelRequirement))]
    public static class ExperienceLevelRequirementOverride
    {
        static void Postfix(int level, ref float __result)
        {
            //Stock calls this as a Zero based index so we have to go +1
            switch (level+1)
            {
                case 2:
                    __result = BetterExperienceSystem.Instance.lv1Target;
                    return;
                case 3:
                    __result = BetterExperienceSystem.Instance.lv2Target;
                    return;
                case 4:
                    __result = BetterExperienceSystem.Instance.lv3Target;
                    return;
                case 5:
                    __result = BetterExperienceSystem.Instance.lv4Target;
                    return;
                case 6:
                    __result = BetterExperienceSystem.Instance.lv5Target;
                    return;
                default: 
                    __result = 0;
                    return;
            }
        }
    }
}