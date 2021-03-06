using System;
using System.Collections.Generic;
using Experience;
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
        public float lv5Target = 229;
        public float lv4Target = 111;
        public float lv3Target = 60;
        public float lv2Target = 22;
        public float lv1Target = 4;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Instance = this;
            //TODO: Move the loading stuff to it's own method and call it in a Scenario Module so player doesn't have to restart KSP to save settings
            ConfigNode cn = ConfigNode.Load(KSPUtil.ApplicationRootPath + "/GameData/BetterExperienceSystem/Levels.cfg");
            if (cn == null)
            {
                Logging.Log("Could not find settings cfg. Initialising with default values", LogLevel.Warning);
                //TODO: Make it save the settings file if not found
                return;
            }


            float.TryParse(cn.GetValue("lv5"), out lv5Target);
            float.TryParse(cn.GetValue("lv4"), out lv4Target);
            float.TryParse(cn.GetValue("lv3"), out lv3Target);
            float.TryParse(cn.GetValue("lv2"), out lv2Target);
            float.TryParse(cn.GetValue("lv1"), out lv1Target);
        }

        public void RegisterNewExperienceType(string name, string typeName, float notHomeValue, float homeValue = 0)
        {
            xpTypes[name] = new BetterExperienceType(name, typeName, notHomeValue, homeValue);
        }
    }
}