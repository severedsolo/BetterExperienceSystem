using System;
using System.Collections.Generic;
using System.Reflection;
using Experience;
using Harmony;
using KSP.UI;
using KSP.UI.Screens;
using KSP.UI.TooltipTypes;
using UnityEngine;

namespace BetterExperienceSystem
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class BetterExperienceSystem : MonoBehaviour
    {
        public static BetterExperienceSystem Instance;
        public Dictionary<string, BetterExperienceType> xpTypes = new Dictionary<string, BetterExperienceType>();
        public float lv5Target = 243;
        public float lv4Target = 151;
        public float lv3Target = 84;
        public float lv2Target = 31;
        public float lv1Target = 5;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
            LoadSettings();
        }
        private void SetupSkillBasedXp()
        {
            KerbalRoster.AddExperienceType("pilotXP", "Completed a flight to ", 1.0f, 1.0f);
            KerbalRoster.AddExperienceType("scientistXP", "Transmitted science from ", 1.0f, 1.0f);
            KerbalRoster.AddExperienceType("engineerXP", "Performed in-flight construction at ", 1.0f, 1.0f);
        }

        private void LoadSettings()
        {
            //TODO: Call it in a Scenario Module so player doesn't have to restart KSP to save settings
            ConfigNode cn = ConfigNode.Load(KSPUtil.ApplicationRootPath + "/GameData/BetterExperienceSystem/Levels.cfg");
            if (cn == null)
            {
                Logging.Log("Could not find settings cfg. Initialising with default values", LogLevel.Warning);
                //TODO: Make it save the settings file if not found
                return;
            }
        //TODO: These probably need to be rebalanced to take into account Trait Specific XP
            float.TryParse(cn.GetValue("lv5"), out lv5Target);
            float.TryParse(cn.GetValue("lv4"), out lv4Target);
            float.TryParse(cn.GetValue("lv3"), out lv3Target);
            float.TryParse(cn.GetValue("lv2"), out lv2Target);
            float.TryParse(cn.GetValue("lv1"), out lv1Target);
        }

        private void Start()
        {
            Logging.Log("Starting Harmony Patcher", LogLevel.Info);
            var harmony = HarmonyInstance.Create("BetterExperienceSystem");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logging.Log("Patching Complete", LogLevel.Info);
            SetupSkillBasedXp();
        }

        public void RegisterNewExperienceType(string name, string typeName, float notHomeValue, float homeValue = 0)
        {
            xpTypes[name] = new BetterExperienceType(name, typeName, notHomeValue, homeValue);
        }
    }
}