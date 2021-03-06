using System.Reflection;
using Harmony;
using UnityEngine;

namespace BetterExperienceSystem
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class HarmonyPatcher : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log("[BetterExperienceSystem]: Starting Patcher");
            var harmony = HarmonyInstance.Create("BetterExperienceSystem");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Debug.Log("[BetterExperienceSystem]: Patching Complete");
        }
    }
}