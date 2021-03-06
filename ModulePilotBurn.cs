using System;
using System.Collections.Generic;
using System.Reflection;
using UniLinq;
using UnityEngine;

namespace BetterExperienceSystem
{
    public class ModulePilotBurn : PartModule
    {
        private List<Propellant> propellantList = new List<Propellant>();
        private ModuleEngines referenceEngine;

        private void Start()
        {
            referenceEngine = part.FindModuleImplementing<ModuleEngines>();
            propellantList = referenceEngine.propellants;
        }

        private void FixedUpdate()
        {
            //TODO: Make this toggleable in Settings
            //TODO: Make pilots get XP for doing a burn (or possibly on recovery?)
            if (referenceEngine.currentThrottle == 0) return;
            //TODO: We can probably optimise this a bit, doing alot of loops every frame
            float pilotModifier = PilotModifier();
            float friendlyBonus = pilotModifier * 100;
            friendlyBonus = (float)Math.Round(friendlyBonus, 0);
            foreach (Propellant p in propellantList)
            {
                //First figure how much fuel we are burning (and turn it negative)
                double propToTake = referenceEngine.getFuelFlow(p, referenceEngine.requestedMassFlow)*Time.fixedDeltaTime*-1;
                propToTake *= pilotModifier;
                part.RequestResource(p.name, propToTake);
            }

            ScreenMessages.PostScreenMessage(Utilities.HighestRankKerbal("FullVesselControlSkill", vessel.protoVessel) + " in command. " + friendlyBonus + "% fuel bonus in effect");
        }
        

        private float PilotModifier()
        {
            int pilotLevel = Utilities.SkillLevel("FullVesselControlSkill", vessel.protoVessel);
            switch (pilotLevel)
            {
                case 5:
                    return 0.10f;
                case 4: 
                    return 0.08f;
                case 3:
                    return 0.06f;
                case 2:
                    return 0.04f;
                case 1:
                    return 0.02f;
                case 0:
                    return 0.00f;
                default:
                    return -0.10f;
            }
        }


    }
}