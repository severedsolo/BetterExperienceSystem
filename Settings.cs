using System.IO;

namespace BetterExperienceSystem
{
    public static class Settings
    {
        private static readonly string path = KSPUtil.ApplicationRootPath + "/GameData/BetterExperienceSystem/PluginData/Settings.cfg";
        private const string SettingsVersion = "0.1";
        public static bool ModEnabled = true;
        public static float Lv5Target = 243;
        public static float Lv4Target = 151;
        public static float Lv3Target = 84;
        public static float Lv2Target = 31;
        public static float Lv1Target = 5;
        public static bool Skills = true; 
        
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

            bool.TryParse("modEnabled", out ModEnabled);
            float.TryParse(cn.GetValue("lv5"), out Lv5Target);
            float.TryParse(cn.GetValue("lv4"), out Lv4Target);
            float.TryParse(cn.GetValue("lv3"), out Lv3Target);
            float.TryParse(cn.GetValue("lv2"), out Lv2Target);
            float.TryParse(cn.GetValue("lv1"), out Lv1Target);
            bool.TryParse(cn.GetValue("skills"), out Skills);
            Logging.Log("Loaded Settings", LogLevel.Info);
        }

        private static void SaveSettings()
        {
            ConfigNode cn = new ConfigNode("Settings");
            cn.AddValue("version", SettingsVersion);
            cn.AddValue("modEnabled", ModEnabled);
            cn.AddValue("lv5", Lv5Target);
            cn.AddValue("lv4", Lv4Target);
            cn.AddValue("lv3", Lv3Target);
            cn.AddValue("lv2", Lv2Target);
            cn.AddValue("lv1", Lv1Target);
            cn.AddValue("skills", Skills);
            cn.Save(path);
            Logging.Log("Saved Settings", LogLevel.Info);
        }
    }
}