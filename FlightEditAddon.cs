using System;
using System.IO;
using KSP.UI.Screens;
using RUI.Icons.Simple;
using UnityEngine;

namespace FlightEdit
{
    [KSPAddon(KSPAddon.Startup.AllGameScenes, false)]
    public class FlightEditAddon : MonoBehaviour
    {        
        public void Start()
        {
            FlightEdit.instance.RegisterButton();
        }
        
    }
}