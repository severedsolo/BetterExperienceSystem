using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetterExperienceSystem
{
    public class ModulePilotBurn : PartModule
    {
        private List<Propellant> propellantList = new List<Propellant>();
        private ModuleEngines referenceEngine;
        private bool showMessage = true;

        private void Start()
        {
            referenceEngine = part.FindModuleImplementing<ModuleEngines>();
            propellantList = referenceEngine.propellants;
        }

        private void FixedUpdate()
        {
            if (!Settings.PilotSkills) return;
            if (referenceEngine.currentThrottle == 0)
            {
                showMessage = true;
                return;
            }
            //TODO: We can probably optimise this a bit, doing a lot of loops every frame
            float pilotModifier = Utilities.SkillModifier("FullVesselControlSkill", vessel.protoVessel);
            if (pilotModifier == Settings.Lv0Boost) pilotModifier = -0.1f;
            float friendlyBonus = pilotModifier * 100;
            friendlyBonus = (float)Math.Round(friendlyBonus, 0);
            foreach (Propellant p in propellantList)
            {
                //First figure how much fuel we are burning (and turn it negative)
                double propToTake = referenceEngine.getFuelFlow(p, referenceEngine.requestedMassFlow)*Time.fixedDeltaTime*-1;
                propToTake *= pilotModifier;
                part.RequestResource(p.name, propToTake);
            }

            if (!showMessage) return;
            ScreenMessages.PostScreenMessage(Utilities.HighestRankKerbal("FullVesselControlSkill", vessel.protoVessel) + " in command. " + friendlyBonus + "% fuel bonus in effect");
            showMessage = false;
        }
        
    }
}