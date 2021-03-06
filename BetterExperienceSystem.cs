using System.Collections.Generic;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace BetterExperienceSystem
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class BetterExperienceSystem : MonoBehaviour
    {
        public static BetterExperienceSystem Instance;
        public readonly Dictionary<string, BetterExperienceType> XpTypes = new Dictionary<string, BetterExperienceType>();


        private void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
            Settings.LoadSettings();
        }
        private static void SetupSkillBasedXp()
        {
            KerbalRoster.AddExperienceType("pilotXP", "Completed a flight to ", 1.0f, 1.0f);
            KerbalRoster.AddExperienceType("scientistXP", "Transmitted science from ", 1.0f, 1.0f);
            KerbalRoster.AddExperienceType("engineerXP", "Performed in-flight construction at ", 1.0f, 1.0f);
        }

        private void Start()
        {
            if (!Settings.ModEnabled) return;
            Logging.Log("Starting Harmony Patcher", LogLevel.Info);
            HarmonyInstance harmony = HarmonyInstance.Create("BetterExperienceSystem");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logging.Log("Patching Complete", LogLevel.Info);
            SetupSkillBasedXp();
            GameEvents.onGameStateLoad.Add(OnLoad);
        }

        private static void OnLoad(ConfigNode data)
        {
            Settings.LoadSettings();
        }

        public void RegisterNewExperienceType(string xpName, string typeName, float notHomeValue, float homeValue = 0)
        {
            XpTypes[xpName] = new BetterExperienceType(xpName, typeName, notHomeValue, homeValue);
        }

        private void OnDisable()
        {
            GameEvents.onGameStateLoad.Remove(OnLoad);
        }
    }
}