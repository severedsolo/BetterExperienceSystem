using Harmony;
using JetBrains.Annotations;

namespace BetterExperienceSystem
{
    [HarmonyPatch(typeof(ProtoCrewMember))]
    [HarmonyPatch(nameof(ProtoCrewMember.ExperienceLevelDelta), MethodType.Getter)]
    [UsedImplicitly]
    public static class ProtoCrewMemberExperienceDeltaOverride
    {
        [UsedImplicitly]
        private static void Postfix(ref float result, ProtoCrewMember instance)
        {
            float currentXp = KerbalRoster.CalculateExperience(instance.careerLog);
            int currentLevel = KerbalRoster.CalculateExperienceLevel(currentXp);
            //Calcs go weird above max level so just return 1.0f;
            if (currentLevel == 5)
            {
                result = 1.0f;
                return;
            }
            float nextLevelXp = KerbalRoster.GetExperienceLevelRequirement(currentLevel+1);
            float previousLevelXp = KerbalRoster.GetExperienceLevelRequirement(currentLevel);
            float xpGainedSinceLastLevel = currentXp - previousLevelXp;
            float xpRequiredToNextLevel = nextLevelXp - previousLevelXp;
            result = xpGainedSinceLastLevel / xpRequiredToNextLevel;
        }
    }
}