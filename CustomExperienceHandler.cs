using System;
using System.Collections.Generic;
using KSP.UI.Screens;
using UniLinq;
using UnityEngine;

namespace BetterExperienceSystem
{
    public class CustomExperienceHandler : MonoBehaviour
    {
        private void Start()
        {
            GameEvents.onVesselRecoveryProcessing.Add(ProcessPilotXp);
            GameEvents.OnScienceRecieved.Add(ProcessScientistXp);
            GameEvents.OnEVAConstructionModePartAttached.Add(OnEvaConstruct);
            GameEvents.OnEVAConstructionModePartDetached.Add(OnEvaConstruct);
            SetupSkillBasedXp();
        }

        private void ProcessPilotXp(ProtoVessel pv, MissionRecoveryDialog dialog, float kscMultiplier)
        {
            Logging.Log("VesselRecoveryProcessing", LogLevel.Info);
            List<ProtoCrewMember> crew = pv.GetVesselCrew();
            for (int i = 0; i < crew.Count; i++)
            {
                ProtoCrewMember p = crew.ElementAt(i);
                if (!p.HasEffect("FullVesselControlSkill")) continue;
                FlightLog log = p.flightLog;
                //No point iterating the whole list when we just want the last flight
                int lastEntry = log.Last().flight;
                string highestScoringBody = String.Empty;
                float highestScore = 0;
                for (int logEntryCount = log.Count - 1; logEntryCount >= 0; logEntryCount--)
                {
                    FlightLog.Entry logEntry = log[logEntryCount];
                    //Once we've checked the last flight we can break
                    //Incidentally, not using ModuleTripLogger for this as the Kerbal might not have gone as far as the craft has
                    //(or may have gone further if they hitched a lift from orbit) 
                    if (logEntry.flight < lastEntry) break;
                    //Pilots get XP for the highest scoring target they visited (General rule is, one bonus XP per flight).
                    //Yes people are going to cheese this and do quick flights, but.. whatever.
                    float recoveryValue = Utilities.BodyFromName(logEntry.target).scienceValues.RecoveryValue;
                    if (recoveryValue <= highestScore) continue;
                    highestScore = recoveryValue;
                    highestScoringBody = logEntry.target;
                }
                p.flightLog.AddEntry("pilotXP", highestScoringBody);
                Logging.Log("Awarded pilotXP to " + p.name + " for reaching " + highestScoringBody, LogLevel.Info);
            }
        }

        private void ProcessScientistXp(float scienceAmount, ScienceSubject subject, ProtoVessel pv, bool reverseEngineered)
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
                int lastFlight = log.Last().flight;
                bool dontAward = false;
                for (int logCount = log.Count - 1; logCount >= 0; logCount--)
                {
                    FlightLog.Entry logEntry = log[logCount];
                    if (logEntry.flight < lastFlight) break;
                    if (AwardXP(logEntry, pv.vesselRef.mainBody.name, "scientistXP", p)) continue;
                    dontAward = true;
                    break;
                }

                if (dontAward) continue;
                p.flightLog.AddEntry("scientistXP", pv.vesselRef.mainBody.name);
                Logging.Log("Awarded scientistXP for "+pv.vesselRef.mainBody.name+" to "+p.displayName, LogLevel.Info);
            }
        }

        private bool AwardXP(FlightLog.Entry logEntry, string currentBody, string xpTypeToCheck, ProtoCrewMember p)
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

        private void OnEvaConstruct(Vessel constructVessel, Part constructPart)
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
                int lastFlight = log.Last().flight;
                bool dontAward = false;
                for (int logCount = log.Count - 1; logCount >= 0; logCount--)
                {
                    FlightLog.Entry logEntry = log[logCount];
                    if (logEntry.flight < lastFlight) break;
                    if (AwardXP(logEntry, v.mainBody.name, "engineerXP", p)) continue;
                    dontAward = true;
                    break;
                }

                if (dontAward) continue;
                p.flightLog.AddEntry("engineerXP", v.mainBody.name);
            }
        }

        private void SetupSkillBasedXp()
        {
            KerbalRoster.AddExperienceType("pilotXP", "Completed a flight to", 1.0f, 1.0f);
            KerbalRoster.AddExperienceType("scientistXP", "Transmitted science from", 1.0f, 1.0f);
            KerbalRoster.AddExperienceType("engineerXP", "Performed in-flight construction at", 1.0f, 1.0f);
        }
    }
}