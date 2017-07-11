using KSP.UI.Screens;
using UnityEngine;

namespace FlightEdit
{
    public class FlightEdit
    {

        public static readonly FlightEdit instance = new FlightEdit();
        private ApplicationLauncherButton _flightEditButton;

        public void RegisterButton()
        {
            if (_flightEditButton != null) return;
            var texture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
            this._flightEditButton = ApplicationLauncher.Instance.AddModApplication(OnCraftEdit, null,
                null, null, null, null, ApplicationLauncher.AppScenes.ALWAYS, texture);
        }

        public void OnCraftEdit()
        {
            if (FlightGlobals.ActiveVessel != null)
            {
                EditorDriver.StartAndLoadVessel(InflightSave.TemporarySaveVessel(FlightGlobals.ActiveVessel), 
                    EditorFacility.VAB);
            }
        }
    }
}