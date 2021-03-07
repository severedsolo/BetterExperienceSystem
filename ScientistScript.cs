using System;
using UnityEngine;

namespace BetterExperienceSystem
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class ScientistScript : MonoBehaviour
    {
        private void Start()
        {
            if (!Settings.ScientistSkills) return;
            //TODO: This will probably break with Kerbalism installed
            GameEvents.OnScienceRecieved.Add(OnScienceReceived);
        }

        private static void OnScienceReceived(float sciAmount, ScienceSubject subject, ProtoVessel pv, bool reverseEngineered)
        {
            if (reverseEngineered) return;
            float sciModifier = 0.1f + Utilities.SkillModifier("ScienceSkill", pv); 
            float scienceToGive = sciAmount*sciModifier;
            if (scienceToGive == 0) return;
            ScreenMessages.PostScreenMessage(Utilities.HighestRankKerbal("ScienceSkill", pv) + " transmitted science. " + Math.Round(scienceToGive, 1) + " extra science awarded");
            ResearchAndDevelopment.Instance.AddScience(scienceToGive, TransactionReasons.ScienceTransmission);
            //TODO: Maybe add a science event here so Bureaucracy picks up on it?
        }

        private void OnDisable()
        {
            GameEvents.OnScienceRecieved.Remove(OnScienceReceived);
        }
    }
}