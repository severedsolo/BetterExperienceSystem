using System;
using System.Collections.Generic;
using UniLinq;
using UnityEngine;

namespace BetterExperienceSystem
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class CustomExperienceHandler : MonoBehaviour
    {
        private void Start()
        {
            if (Settings.PilotSkills)
            {
                GameEvents.OnVesselRecoveryRequested.Add(ProcessPilotXp);
                GameEvents.onVesselSOIChanged.Add(OnSOIChange);
            }

            if (Settings.ScientistSkills)
            {
                GameEvents.OnScienceRecieved.Add(ProcessScientistXp);
            }

            if (Settings.EngineerSkills)
            {
                GameEvents.OnEVAConstructionModePartAttached.Add(OnEvaConstruct);
                GameEvents.OnEVAConstructionModePartDetached.Add(OnEvaConstruct);
            }
        }

        private static void OnSOIChange(GameEvents.HostedFromToAction<Vessel, CelestialBody> eventData)
        {
            if (!HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().ImmediateLevelUp) return;
            ProcessPilotXp(eventData.host);
        }

        private static void ProcessPilotXp(Vessel v)
        {
            Logging.Log("VesselRecoveryProcessing", LogLevel.Info);
            List<ProtoCrewMember> crew = v.GetVesselCrew();
            for (int i = 0; i < crew.Count; i++)
            {
                ProtoCrewMember p = crew.ElementAt(i);
                if (!p.HasEffect("FullVesselControlSkill")) continue;
                FlightLog log = p.flightLog;
                string highestScoringBody = String.Empty;
                float highestScore = 0;
                for (int logEntryCount = log.Count - 1; logEntryCount >= 0; logEntryCount--)
                {
                    FlightLog.Entry logEntry = log[logEntryCount];
                    //Pilots get XP for the highest scoring target they visited (General rule is, one bonus XP per flight).
                    //Yes people are going to cheese this and do quick flights, but.. whatever.
                    if (logEntry.target == String.Empty) continue;
                    float recoveryValue = Utilities.BodyFromName(logEntry.target).scienceValues.RecoveryValue;
                    if (recoveryValue <= highestScore) continue;
                    highestScore = recoveryValue;
                    highestScoringBody = logEntry.target;
                }
                if (highestScoringBody == String.Empty) continue;
                p.flightLog.AddEntry("pilotXP", highestScoringBody);
                Logging.Log("Awarded pilotXP to " + p.name + " for reaching " + highestScoringBody, LogLevel.Info);
            }
        }

        private static void ProcessScientistXp(float scienceAmount, ScienceSubject subject, ProtoVessel pv, bool reverseEngineered)
        {
            if (reverseEngineered) return;
            if (scienceAmount < 0.1f) return;
            if (pv.vesselRef == null) return;
            List<ProtoCrewMember> crew = pv.GetVesselCrew();
            for (int i = 0; i < crew.Count; i++)
            {
                ProtoCrewMember p = crew.ElementAt(i);
                if (!p.HasEffect("ScienceSkill")) continue;
                FlightLog log = p.flightLog;
                bool dontAward = false;
                for (int logCount = log.Count - 1; logCount >= 0; logCount--)
                {
                    FlightLog.Entry logEntry = log[logCount];
                    if (AwardXp(logEntry, pv.vesselRef.mainBody.name, "scientistXP", p)) continue;
                    dontAward = true;
                    break;
                }

                if (dontAward) continue;
                p.flightLog.AddEntry("scientistXP", pv.vesselRef.mainBody.name);
                Logging.Log("Awarded scientistXP for "+pv.vesselRef.mainBody.name+" to "+p.displayName, LogLevel.Info);
            }
        }

        private static bool AwardXp(FlightLog.Entry logEntry, string currentBody, string xpTypeToCheck, ProtoCrewMember p)
        {
            if (logEntry.type != xpTypeToCheck) return true;
            //If they already picked up this XP on this flight, no duplicates.
            if (logEntry.target == currentBody) return false;
            //Now, is this entry BETTER than the one they already claimed?
            if (Utilities.BodyFromName(logEntry.target).scienceValues.RecoveryValue > Utilities.BodyFromName(currentBody).scienceValues.RecoveryValue) return false;
            //Otherwise, assuming this one is better, remove the old entry and award them the new one.
            p.flightLog.Entries.Remove(logEntry);
            return true;
        }

        private static void OnEvaConstruct(Vessel constructVessel, Part constructPart)
        {
            List<Vessel> loadedVessels = FlightGlobals.VesselsLoaded;
            for (int i = 0; i < loadedVessels.Count; i++)
            {
                Vessel v = loadedVessels.ElementAt(i);
                KerbalEVA k = v.FindPartModuleImplementing<KerbalEVA>();
                if (k == null) continue;
                ProtoCrewMember p = k.vessel.GetVesselCrew().FirstOrDefault();
                if (!p.HasEffect("RepairSkill")) continue;
                FlightLog log = p.flightLog;
                bool dontAward = false;
                for (int logCount = log.Count - 1; logCount >= 0; logCount--)
                {
                    FlightLog.Entry logEntry = log[logCount];
                    if (AwardXp(logEntry, v.mainBody.name, "engineerXP", p)) continue;
                    dontAward = true;
                    break;
                }

                if (dontAward) continue;
                p.flightLog.AddEntry("engineerXP", v.mainBody.name);
                Logging.Log("Awarded engineerXp to " + p.name + " for orbital construction around " + v.mainBody.name, LogLevel.Info);
            }
        }

        private void OnDisable()
        {
            GameEvents.OnVesselRecoveryRequested.Remove(ProcessPilotXp);
            GameEvents.OnScienceRecieved.Remove(ProcessScientistXp);
            GameEvents.OnEVAConstructionModePartAttached.Remove(OnEvaConstruct);
            GameEvents.OnEVAConstructionModePartDetached.Remove(OnEvaConstruct);
            GameEvents.onVesselSOIChanged.Remove(OnSOIChange);
        }
    }
}