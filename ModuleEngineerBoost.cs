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
            if (!Settings.ModEnabled) return;
            if (!Settings.Skills) return;
            evaKerbal = vessel.GetVesselCrew().FirstOrDefault();
            if (evaKerbal == null) return;
            if (!evaKerbal.HasEffect("RepairSkill")) return;
            boost = PhysicsGlobals.ConstructionWeightLimit * evaKerbal.experienceLevel * 0.02f;
            PhysicsGlobals.ConstructionWeightLimit += boost;
            Logging.Log("Added " + boost + " to EVA Construction Limit. Limit is now "+PhysicsGlobals.ConstructionWeightLimit, LogLevel.Info);
            ScreenMessages.PostScreenMessage(evaKerbal.displayName + " entered the field. Construction Limit is now " + Math.Round(PhysicsGlobals.ConstructionWeightLimit, 0));
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