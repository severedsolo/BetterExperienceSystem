using System;
using System.Collections.Generic;
using UniLinq;

namespace BetterExperienceSystem
{
    public static class Utilities
    {

        private static readonly float[] stockLevels = {2.0f, 8.0f, 16.0f, 32.0f, 64.0f};
        private static readonly string[] stockTypes = {"PlantFlag", "Land", "Flight", "Suborbit", "Flyby", "Escape", "Land"};

        public static float GetXpForType(string xpName, string bodyName)
        {
            BetterExperienceSystem.Instance.XpTypes.TryGetValue(xpName, out BetterExperienceType xpType);
            if (xpType == null) return 0;
            //If a mod has added an XP type that gives exactly enough XP to level a kerbal up, assume that was the intention (only 2 I know of are Strategia and MKS which both use it explicitly for this purpose)
            if (!stockTypes.Contains(xpType.XpName) && stockLevels.Contains(xpType.XpHomeValue)) return ConvertedStockLevelXp(xpType.XpHomeValue);
            if (FlightGlobals.GetHomeBody().name == bodyName) return xpType.XpHomeValue;
            return xpType.XpNotHomeValue * BodyFromName(bodyName).scienceValues.RecoveryValue;
        }

        public static string HighestRankKerbal(string skill, ProtoVessel pv)
        {
            int level = -1;
            string nameToReturn = String.Empty;
            if (skill == "FullVesselControlSkill")
            {
                level = Math.Max(level, GetSasLevel(pv));
            }
            List<ProtoCrewMember> crew = pv.GetVesselCrew();
            for (int i = 0; i < crew.Count; i++)
            {
                ProtoCrewMember p = crew.ElementAt(i);
                if (!p.HasEffect(skill)) continue;
                if (p.experienceLevel <= level) continue;
                level = p.experienceLevel;
                nameToReturn = p.displayName;
            }

            // ReSharper disable once InvertIf
            if (nameToReturn == String.Empty)
            {
                if (crew.Count == 0) nameToReturn = "Probe Core";
                else nameToReturn = crew.ElementAt(0).displayName;
            }
            return nameToReturn;
        }

        private static int GetSasLevel(ProtoVessel pv)
        {
            int level = -1;
            for (int i = 0; i < pv.protoPartSnapshots.Count; i++)
            {
                List<ProtoPartModuleSnapshot> moduleList = pv.protoPartSnapshots.ElementAt(i).modules;
                for (int moduleCount = 0; moduleCount < moduleList.Count; moduleCount++)
                {
                    ProtoPartModuleSnapshot pps = moduleList.ElementAt(moduleCount);
                    ModuleSAS sas = pps.moduleRef as ModuleSAS;
                    // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
                    if (sas == null) continue;
                    level = Math.Max(sas.SASServiceLevel, level);
                }
            }

            return level;
        }

        public static int SkillLevel(string skillToCheck, ProtoVessel pv)
        {
            int level = -1;
            if (pv == null) return level;
            if(skillToCheck == "FullVesselControlSkill") return PilotLevel(pv);
            List<ProtoCrewMember> crew = pv.GetVesselCrew();
            for (int i = 0; i < crew.Count; i++)
            {
                ProtoCrewMember p = crew.ElementAt(i);
                if (!p.HasEffect(skillToCheck)) continue;
                level = Math.Max(p.experienceLevel, level);
            }

            return level;
        }

        private static int PilotLevel(ProtoVessel pv)
        {
            int pilotLevel = Math.Max(GetSasLevel(pv), -1);
            List<ProtoCrewMember> crew = pv.GetVesselCrew();
            for (int i = 0; i < crew.Count; i++)
            {
                ProtoCrewMember p = crew.ElementAt(i);
                if (!p.HasEffect("FullVesselControlSkill")) continue;
                pilotLevel = Math.Max(p.experienceLevel, pilotLevel);
            }

            return pilotLevel;
        }
        private static float ConvertedStockLevelXp(float value)
        {
            for (int i = 0; i < stockLevels.Length; i++)
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (stockLevels[i] == value)
                {
                    switch (i+1)
                    {
                        case 1:
                            return Settings.Lv1Target;
                        case 2: 
                            return Settings.Lv2Target;
                        case 3:
                            return Settings.Lv3Target;
                        case 4:
                            return Settings.Lv4Target;
                        case 5:
                            return Settings.Lv5Target;
                    }
                }
            }
            return 0;
        }

        public static CelestialBody BodyFromName(string bodyName)
        {
            for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
            {
                CelestialBody cb = FlightGlobals.Bodies.ElementAt(i);
                if (cb.name == bodyName) return cb;
            }

            return null;
        }
    }
}