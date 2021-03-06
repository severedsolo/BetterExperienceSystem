using System;
using UniLinq;
using UnityEngine;

namespace BetterExperienceSystem
{
    public class ModuleEngineerBoost : PartModule
    {
        private double boost = 0;
        private ProtoCrewMember evaKerbal;
        private void Start()
        {
            evaKerbal = vessel.GetVesselCrew().FirstOrDefault();
            if (evaKerbal == null) return;
            if (!evaKerbal.HasEffect("RepairSkill")) return;
            boost = PhysicsGlobals.ConstructionWeightLimit * evaKerbal.experienceLevel * 0.02f;
            int friendlyBonus = (int)Math.Round(boost, 0);
            PhysicsGlobals.ConstructionWeightLimit += boost;
            Logging.Log("Added " + boost + " to EVA Construction Limit. Limit is now "+PhysicsGlobals.ConstructionWeightLimit, LogLevel.Info);
            ScreenMessages.PostScreenMessage(evaKerbal.displayName + " entered the field. Construction Limit is now " + PhysicsGlobals.ConstructionWeightLimit);
            //TODO: Add extra XP for doing construction work
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