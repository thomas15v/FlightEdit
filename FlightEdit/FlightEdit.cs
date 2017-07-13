using System;
using System.Collections.Generic;
using KSP.UI.Screens;
using UnityEngine;

namespace FlightEdit.FlightEdit
{
    public class FlightEdit
    {

        public static readonly FlightEdit Instance = new FlightEdit();
        public Guid Vesselid { get; private set; }
        public bool EdditingVessel { get; private set; }

        public void OnFlight()
        {
            InputLockManager.ClearControlLocks();
            
        }

        public void OnEdit()
        {
            if (!EdditingVessel) return;
            EditorLogic.fetch.launchBtn.onClick.RemoveListener(EditorLogic.fetch.launchVessel);
            EditorLogic.fetch.launchBtn.onClick.AddListener(OnCraftLaunch);
        }

        public void OnEditDestroy()
        {
            if (!EdditingVessel) return;
            EdditingVessel = false;
        }
        
        
        public void OnCraftEdit()
        {
            if (FlightGlobals.ActiveVessel == null) return;
            GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE);
            EdditingVessel = true;
            Vesselid = FlightGlobals.ActiveVessel.protoVessel.vesselID;
            EditorDriver.StartAndLoadVessel(InflightSave.TemporarySaveVessel(FlightGlobals.ActiveVessel), 
                EditorFacility.VAB);
        }

        private void OnCraftLaunch()
        {
            var game = GamePersistence.LoadGame(FlightDriver.StateFileToLoad, HighLogic.SaveFolder, true, true);
            var vesselid = game.flightState.protoVessels.FindIndex(v => v.vesselID.Equals(Vesselid));
            var vessel = game.flightState.protoVessels[vesselid];
            
            foreach (var crewMember in vessel.GetVesselCrew())
            {
                var part = EditorLogic.fetch.ship.parts.Find(p => p.protoModuleCrew.Count < p.CrewCapacity);
                if (part == null)
                {
                    Debug.LogWarning("We dit not had enough space for this kerbal");
                }
                else
                {
                    part.AddCrewmemberAt(crewMember, part.protoModuleCrew.Count);
                }
            }
            
            var newVessel = GetVesselFromShipConstruct(EditorLogic.fetch.ship, vessel, game); 
            vessel.protoPartSnapshots = newVessel.protoPartSnapshots;
           
            GamePersistence.SaveGame(game, FlightDriver.StateFileToLoad, HighLogic.SaveFolder, SaveMode.OVERWRITE);
            FlightDriver.StartAndFocusVessel(game, vesselid);
            Debug.Log("Editing Vessel and switching to it");
        }

        private static ProtoVessel GetVesselFromShipConstruct(ShipConstruct shipConstruct, ProtoVessel oldVessel, 
            Game game)
        {
            foreach (var p in shipConstruct.parts)
            {
                p.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
                p.missionID = oldVessel.protoPartSnapshots[0].missionID;
                p.launchID = oldVessel.protoPartSnapshots[0].launchID;
                p.flagURL = oldVessel.protoPartSnapshots[0].flagURL ?? HighLogic.CurrentGame.flagURL;
                p.temperature = 1.0;
                // LITARLY NO CLUE WHY NO CODE IN SQUAD IS EXECUTING THIS (this little piece of shit took me 2 days)
               
                p.UpdateOrgPosAndRot(shipConstruct.parts[0]);
            }
            
            var empty = new ConfigNode();
            var dummyProto = new ProtoVessel(empty, null);
            var dummyVessel = new Vessel();
            dummyVessel.parts = shipConstruct.parts;
            dummyProto.vesselRef = dummyVessel;
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            foreach (Part p in shipConstruct.parts)
            {
                dummyProto.protoPartSnapshots.Add(new ProtoPartSnapshot(p, dummyProto));
            }
            // ReSharper disable once SuggestVarOrType_SimpleTypes
            foreach (ProtoPartSnapshot p in dummyProto.protoPartSnapshots)
            {
                p.storePartRefs();
            }

            return dummyProto;
        }
    }
}