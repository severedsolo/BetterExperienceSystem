using Harmony;
using JetBrains.Annotations;

namespace BetterExperienceSystem
{
    [HarmonyPatch(typeof(KerbalRoster))]
    [HarmonyPatch(nameof(KerbalRoster.CalculateExperienceLevel))]
    [UsedImplicitly]
    public static class CalculateExperienceLevelOverride
    {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private static void Postfix(ref float xp, ref int __result)
        {
            for (int i = 5; i >= 0; i--)
            {
                switch (i)
                {
                    case 5:
                        if (xp >= Settings.Lv5Target)
                        {
                            __result = 5;
                            return;
                        }
                        break;
                    case 4:
                        if (xp >= Settings.Lv4Target)
                        {
                            __result = 4;
                            return;
                        }
                        break;
                    case 3:
                        if (xp >= Settings.Lv3Target)
                        {
                            __result = 3;
                            return;
                        }
                        break;
                    case 2:
                        if (xp >= Settings.Lv2Target)
                        {
                            __result = 2;
                            return;
                        }
                        break;
                    case 1:
                        if (xp >= Settings.Lv1Target)
                        {
                            __result = 1;
                            return;
                        }
                        break;
                    default:
                        __result = 0;
                        return;
                }
            }
        }

        
    }
}