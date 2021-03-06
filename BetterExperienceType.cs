using KSP.Localization;

namespace BetterExperienceSystem
{
    public class BetterExperienceType
    {
        public readonly string XpTypeName;
        public readonly float XpNotHomeValue;
        public readonly float XpHomeValue;

        public string XpName { get; }

        public BetterExperienceType(string name, string typeName, float notHomeValue, float homeValue = 0)
        {
            XpName = name;
            if (typeName.Contains("autoLOC"))
            {
                XpTypeName = Localizer.Format(typeName);
                XpTypeName = XpTypeName.Substring(0, XpTypeName.IndexOf('<'));
            }
            else XpTypeName = typeName;

            XpNotHomeValue = notHomeValue;
            XpHomeValue = homeValue;
            Logging.Log("New XP Type logged: " + XpName + " " + XpTypeName + " " + XpNotHomeValue + " " + XpHomeValue, LogLevel.Info);
        }
    }
}