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
        private static void Postfix(ref float xp, ref int result)
        {
            for (int i = 5; i >= 0; i--)
            {
                switch (i)
                {
                    case 5:
                        if (xp >= Settings.Lv5Target)
                        {
                            result = 5;
                            return;
                        }
                        break;
                    case 4:
                        if (xp >= Settings.Lv4Target)
                        {
                            result = 4;
                            return;
                        }
                        break;
                    case 3:
                        if (xp >= Settings.Lv3Target)
                        {
                            result = 3;
                            return;
                        }
                        break;
                    case 2:
                        if (xp >= Settings.Lv2Target)
                        {
                            result = 2;
                            return;
                        }
                        break;
                    case 1:
                        if (xp >= Settings.Lv1Target)
                        {
                            result = 1;
                            return;
                        }
                        break;
                    default:
                        result = 0;
                        return;
                }
            }
        }

        
    }
}