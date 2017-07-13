using KSP.UI.Screens;
using UnityEngine;

namespace FlightEdit
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class FlightAddon : MonoBehaviour
    {        
        
        private ApplicationLauncherButton _flightEditButton;
        
        public void Start()
        {
            Debug.Log("[FLIGHTEDIT] Starting Flight");
            FlightEdit.FlightEdit.Instance.OnFlight();
            app_launcher_ready();
        }

        private void OnDisable()
        {
            ApplicationLauncher.Instance.RemoveModApplication(_flightEditButton);
        }

        private void app_launcher_ready()
        {
            var texture = new Texture2D(36, 36, TextureFormat.RGBA32, false);
            _flightEditButton = ApplicationLauncher.Instance.AddModApplication(
                FlightEdit.FlightEdit.Instance.OnCraftEdit, 
                null, null, null, null, null, 
                ApplicationLauncher.AppScenes.FLIGHT, texture);      
        }        
    }
    
    [KSPAddon(KSPAddon.Startup.EditorVAB, false)]
    public class EdditAddon : MonoBehaviour
    {        
        public void Start()
        {
            Debug.Log("[FLIGHTEDIT] Starting editor");
            FlightEdit.FlightEdit.Instance.OnEdit();
        }

        public void OnDisable()
        {
            Debug.Log("[FLIGHTEDIT] Stopping editor");
            FlightEdit.FlightEdit.Instance.OnEditDestroy();
        }
        
    }
}