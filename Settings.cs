using System.IO;

namespace BetterExperienceSystem
{
    public static class Settings
    {
        private static readonly string path = KSPUtil.ApplicationRootPath + "/GameData/BetterExperienceSystem/PluginData/Settings.cfg";
        private const string SettingsVersion = "0.1";
        public static float Lv5Target = 243;
        public static float Lv4Target = 151;
        public static float Lv3Target = 84;
        public static float Lv2Target = 31;
        public static float Lv1Target = 5;
        public static bool PilotSkills = true;
        public static bool ScientistSkills = true;
        public static bool EngineerSkills = true;
        public static float Lv5Boost = 0.1f;
        public static float Lv4Boost = 0.08f;
        public static float Lv3Boost = 0.06f;
        public static float Lv2Boost = 0.04f;
        public static float Lv1Boost = 0.02f;
        public static float Lv0Boost = 0.0f;
        
        public static void LoadSettings()
        {
            if (!Directory.Exists(KSPUtil.ApplicationRootPath + "GameData/BetterExperienceSystem/PluginData")) Directory.CreateDirectory(KSPUtil.ApplicationRootPath + "GameData/BetterExperienceSystem/PluginData");
            ConfigNode cn = ConfigNode.Load(path);
            if (cn == null)
            {
                Logging.Log("Could not find settings cfg. Initialising with default values", LogLevel.Warning);
                SaveSettings();
                return;
            }
            float.TryParse(cn.GetValue("xpToLv5"), out Lv5Target);
            float.TryParse(cn.GetValue("xpToLv4"), out Lv4Target);
            float.TryParse(cn.GetValue("xpToLv3"), out Lv3Target);
            float.TryParse(cn.GetValue("xpToLv2"), out Lv2Target);
            float.TryParse(cn.GetValue("xpToLv1"), out Lv1Target);
            float.TryParse(cn.GetValue("lv5Boost"), out Lv5Boost);
            float.TryParse(cn.GetValue("lv4Boost"), out Lv4Boost);
            float.TryParse(cn.GetValue("lv3Boost"), out Lv3Boost);
            float.TryParse(cn.GetValue("lv2Boost"), out Lv2Boost);
            float.TryParse(cn.GetValue("lv1Boost"), out Lv1Boost);
            float.TryParse(cn.GetValue("lv0Boost"), out Lv0Boost);
            bool.TryParse(cn.GetValue("pilotSkills"), out PilotSkills);
            bool.TryParse(cn.GetValue("scientistSkills"), out ScientistSkills);
            bool.TryParse(cn.GetValue("engineerSkills"), out EngineerSkills);
            Logging.Log("Loaded Settings", LogLevel.Info);
        }

        private static void SaveSettings()
        {
            ConfigNode cn = new ConfigNode("Settings");
            cn.AddValue("version", SettingsVersion);
            cn.AddValue("xpToLv5", Lv5Target);
            cn.AddValue("xpToLv4", Lv4Target);
            cn.AddValue("xpToLv3", Lv3Target);
            cn.AddValue("xpToLv2", Lv2Target);
            cn.AddValue("xpToLv1", Lv1Target);
            cn.AddValue("lv5Boost", Lv5Boost);
            cn.AddValue("lv4Boost", Lv4Boost);
            cn.AddValue("lv3Boost", Lv3Boost);
            cn.AddValue("lv2Boost", Lv2Boost);
            cn.AddValue("lv1Boost", Lv1Boost);
            cn.AddValue("lv0Boost", Lv0Boost);
            cn.AddValue("pilotSkills", PilotSkills);
            cn.AddValue("scientistSkills", ScientistSkills);
            cn.AddValue("engineerSkills", EngineerSkills);
            cn.Save(path);
            Logging.Log("Saved Settings", LogLevel.Info);
        }
    }
}