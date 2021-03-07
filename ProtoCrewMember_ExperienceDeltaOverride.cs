using Harmony;
using JetBrains.Annotations;
// ReSharper disable InconsistentNaming

namespace BetterExperienceSystem
{
    [HarmonyPatch(typeof(ProtoCrewMember))]
    [HarmonyPatch(nameof(ProtoCrewMember.ExperienceLevelDelta), MethodType.Getter)]
    [UsedImplicitly]
    public static class ProtoCrewMemberExperienceDeltaOverride
    {
        [UsedImplicitly]
        // ReSharper disable once RedundantAssignment
        private static void Postfix(ref float __result, ProtoCrewMember __instance)
        {
            float currentXp = KerbalRoster.CalculateExperience(__instance.careerLog);
            int currentLevel = KerbalRoster.CalculateExperienceLevel(currentXp);
            //Calcs go weird above max level so just return 1.0f;
            if (currentLevel == 5)
            {
                __result = 1.0f;
                return;
            }
            float nextLevelXp = KerbalRoster.GetExperienceLevelRequirement(currentLevel+1);
            float previousLevelXp = KerbalRoster.GetExperienceLevelRequirement(currentLevel);
            float xpGainedSinceLastLevel = currentXp - previousLevelXp;
            float xpRequiredToNextLevel = nextLevelXp - previousLevelXp;
            __result = xpGainedSinceLastLevel / xpRequiredToNextLevel;
        }
    }
}