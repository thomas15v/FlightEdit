//This code is taken from InflightShipSave by Claw, using the CC-BY-NC-SA license.
//See https://github.com/ClawKSP/InflightShipSave

//Actually I found this code from Kerbal_Construction_Time as well. All bundled in this utility class. Thanks guys ;)


using UnityEngine;

namespace FlightEdit
{
    public class InflightSave
    {
        public static string TemporarySaveVessel(Vessel VesselToSave)
        {
            string ShipName = VesselToSave.vesselName;

            ShipConstruct ConstructToSave = new ShipConstruct(ShipName, "", VesselToSave.parts[0]);

            Quaternion OriginalRotation = VesselToSave.vesselTransform.rotation;
            Vector3 OriginalPosition = VesselToSave.vesselTransform.position;

            VesselToSave.SetRotation(new Quaternion(0, 0, 0, 1));
            Vector3 ShipSize = ShipConstruction.CalculateCraftSize(ConstructToSave);
            VesselToSave.SetPosition(new Vector3(0, ShipSize.y + 2, 0));

            ConfigNode CN = new ConfigNode("ShipConstruct");
            CN = ConstructToSave.SaveShip();

            VesselToSave.SetRotation(OriginalRotation);
            VesselToSave.SetPosition(OriginalPosition);

            string filename = UrlDir.ApplicationRootPath + "GameData/FlightEdit/" + ShipName + "_Rescued.craft";
            CN.Save(filename);
            return filename;
        }
        
        private static void CleanEditorNodes (ConfigNode CN)
        {

            CN.SetValue("EngineIgnited", "False");
            CN.SetValue("currentThrottle", "0");
            CN.SetValue("Staged", "False");
            CN.SetValue("sensorActive", "False");
            CN.SetValue("throttle", "0");
            CN.SetValue("generatorIsActive", "False");
            CN.SetValue("persistentState", "STOWED");

            string ModuleName = CN.GetValue("name");

            // Turn off or remove specific things
            if ("ModuleScienceExperiment" == ModuleName)
            {
                CN.RemoveNodes("ScienceData");
            }
            else if ("ModuleScienceExperiment" == ModuleName)
            {
                CN.SetValue("Inoperable", "False");
                CN.RemoveNodes("ScienceData");
            }
            else if ("Log" == ModuleName)
            {
                CN.ClearValues();
            }


            for (int IndexNodes = 0; IndexNodes < CN.nodes.Count; IndexNodes++)
            {
                CleanEditorNodes (CN.nodes[IndexNodes]);
            }
        }
    }
}