using KSP.Localization;
using KSP.UI;
using UniLinq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace BetterExperienceSystem
{
    public class BetterExperienceType
    {
        private string xpName;
        public string xpTypeName;
        public float xpNotHomeValue;
        public float xpHomeValue;

        public string XpName => xpName;

        public BetterExperienceType(string name, string typeName, float notHomeValue, float homeValue = 0)
        {
            xpName = name;
            if (typeName.Contains("autoLOC"))
            {
                xpTypeName = Localizer.Format(typeName);
                xpTypeName = xpTypeName.Substring(0, xpTypeName.IndexOf('<'));
            }
            else xpTypeName = typeName;

            xpNotHomeValue = notHomeValue;
            xpHomeValue = homeValue;
            Debug.Log("[BetterExperienceLevel]: New XP Type logged: " + xpName + " " + xpTypeName + " " + xpNotHomeValue + " " + xpHomeValue);
        }
    }
}