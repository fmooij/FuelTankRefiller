using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FTR
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class Class1 : MonoBehaviour
    {
        public void Start()
        {
            Debug.Log("[FuelTankRefiller] Loaded");
            GameEvents.onFlightReady.Add(OnFlightStart);
        }

        public void OnDestroy()
        {
            GameEvents.onFlightReady.Remove(OnFlightStart);
        }
        bool IsFuelTank(Part p)
        {
            return p.Resources.Any(r =>
                r.resourceName == "LiquidFuel" ||
                r.resourceName == "Oxidizer" ||
                r.resourceName == "MonoPropellant" ||
                r.resourceName == "LqdHydrogen" // Incase mods are used that add new fuel types
            );
        }

        private void OnFlightStart()
        {
            Debug.Log("[FuelTankRefiller] Flight started");
            foreach (Vessel v in FlightGlobals.VesselsLoaded)
            {
                foreach(Part p in v.Parts)
                {
                    if (!p.Modules.Contains("RefillButtonPartModule") && IsFuelTank(p))
                    {
                        p.AddModule("RefillButtonPartModule");
                        Debug.Log($"[FuelTankRefiller] Added RefillButtonPartModule to part {p.partInfo.title} in vessel {v.vesselName}");
                    }
                }
            }
        }
    }

    public class RefillButtonPartModule : PartModule
    {
        [KSPEvent(guiName = "Refill Fuel", guiActive = true, guiActiveEditor = false)]
        public void Refill()
        {
            foreach (PartResource r in part.Resources)
            {
                r.amount = r.maxAmount;
            }
        }
    }
}
