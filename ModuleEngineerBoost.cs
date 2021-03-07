using System;
using System.Diagnostics.CodeAnalysis;
using UniLinq;

namespace BetterExperienceSystem
{
    [SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
    public class ModuleEngineerBoost : PartModule
    {
        private double boost = 0;
        private ProtoCrewMember evaKerbal;
        private void Start()
        {
            if (!Settings.EngineerSkills) return;
            evaKerbal = vessel.GetVesselCrew().FirstOrDefault();
            if (evaKerbal == null) return;
            if (!evaKerbal.HasEffect("RepairSkill")) return;
            //EVA Kerbals don't have a ProtoVessel so can't use the standard level finder here.
            //On the plus side we also only need to worry about one "Crew Member" (the EVA Kerbal themselves) so can just check their Experience Level
            boost = PhysicsGlobals.ConstructionWeightLimit * EngineerModifier(evaKerbal.experienceLevel);
            PhysicsGlobals.ConstructionWeightLimit += boost;
            Logging.Log("Added " + boost + " to EVA Construction Limit. Limit is now "+PhysicsGlobals.ConstructionWeightLimit, LogLevel.Info);
            ScreenMessages.PostScreenMessage(evaKerbal.displayName + " entered the field. Construction Limit is now " + Math.Round(PhysicsGlobals.ConstructionWeightLimit, 0));
        }

        private double EngineerModifier(int evaKerbalExperienceLevel)
        {
            switch (evaKerbalExperienceLevel)
            {
                case 5:
                    return Settings.Lv5Boost;
                case 4:
                    return Settings.Lv4Boost;
                case 3:
                    return Settings.Lv3Boost;
                case 2:
                    return Settings.Lv2Boost;
                case 1:
                    return Settings.Lv1Boost;
                default:
                    return Settings.Lv0Boost;
            }
        }

        private void OnDisable()
        {
            if (evaKerbal == null) return;
            if (!evaKerbal.HasEffect("RepairSkill")) return;
            PhysicsGlobals.ConstructionWeightLimit -= boost;
            Logging.Log("Removed " + boost + " from EVA Construction Limit. Limit is now "+PhysicsGlobals.ConstructionWeightLimit, LogLevel.Info);
            ScreenMessages.PostScreenMessage(evaKerbal.displayName + " left the field. Construction Limit is now " + PhysicsGlobals.ConstructionWeightLimit);
        }
    }
}