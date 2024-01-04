using Exiled.API.Features.Core;
using Exiled.API.Features.Pickups;
using Exiled.API.Interfaces;
using System;
using BasePickup = InventorySystem.Items.Pickups.ItemPickupBase;

namespace TotalCleaner.Types
{
    public class CleanablePickup
    {
        public Pickup Pickup { 
            get {
                return Pickup.Get(PickupSerial);
            }
        }
        public ushort PickupSerial { get; }
        public bool HasBeenRemoved { get; set; }

        public CleanablePickup (Pickup pickup)
        {
            PickupSerial = pickup.Serial;
            HasBeenRemoved = false;

            if (Pickup != null && !Pickup.IsSpawned)
            {
                Pickup.Rigidbody.WakeUp();
                Pickup.Spawn();
            };
        }
    }
}
