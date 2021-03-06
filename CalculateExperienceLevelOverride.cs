using Harmony;
using UnityEngine;

namespace BetterExperienceSystem
{
    [HarmonyPatch(typeof(KerbalRoster))]
    [HarmonyPatch(nameof(KerbalRoster.CalculateExperienceLevel))]
    public static class CalculateExperienceLevelOverride
    {
        static void Postfix(ref float xp, ref int __result)
        {
            for (int i = 5; i >= 0; i--)
            {
                switch (i)
                {
                    case 5:
                        if (xp >= BetterExperienceSystem.Instance.lv5Target)
                        {
                            __result = 5;
                            return;
                        }
                        break;
                    case 4:
                        if (xp >= BetterExperienceSystem.Instance.lv4Target)
                        {
                            __result = 4;
                            return;
                        }
                        break;
                    case 3:
                        if (xp >= BetterExperienceSystem.Instance.lv3Target)
                        {
                            __result = 3;
                            return;
                        }
                        break;
                    case 2:
                        if (xp >= BetterExperienceSystem.Instance.lv2Target)
                        {
                            __result = 2;
                            return;
                        }
                        break;
                    case 1:
                        if (xp >= BetterExperienceSystem.Instance.lv1Target)
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