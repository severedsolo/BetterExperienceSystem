using System;
using UnityEngine;

namespace BetterExperienceSystem
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class ScientistScript : MonoBehaviour
    {
        private void Start()
        {
            if (!Settings.ModEnabled) return;
            if (!Settings.Skills) return;
            //TODO: This will probably break with Kerbalism installed
            GameEvents.OnScienceRecieved.Add(OnScienceReceived);
        }

        private static void OnScienceReceived(float sciAmount, ScienceSubject subject, ProtoVessel pv, bool reverseEngineered)
        {
            if (reverseEngineered) return;
            float scienceToGive = 0;
            switch (Utilities.SkillLevel("ScienceSkill", pv))
            {
                case 5:
                    scienceToGive = sciAmount * 0.20f;
                    break;
                case 4:
                    scienceToGive = sciAmount * 0.18f;
                    break;
                case 3:
                    scienceToGive = sciAmount * 0.16f;
                    break;
                case 2:
                    scienceToGive = sciAmount * 0.14f;
                    break;
                case 1:
                    scienceToGive = sciAmount * 0.12f;
                    break;
                case 0:
                    scienceToGive = sciAmount * 0.10f;
                    break;
                default:
                    scienceToGive = 0;
                    break;
            }
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