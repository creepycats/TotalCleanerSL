using Exiled.API.Features.Core;
using Exiled.API.Features.Pickups;
using Exiled.API.Interfaces;
using System;
using UnityEngine;
using BasePickup = InventorySystem.Items.Pickups.ItemPickupBase;

namespace TotalCleaner.Types
{
    public class UnloadedPickup
    {
        public Pickup Pickup
        {
            get
            {
                return Pickup.Get(PickupSerial);
            }
        }
        public Vector3 Position { get; }
        public ushort PickupSerial { get; }
        public long UnloadTime { get; set; }
        public bool HasBeenRemoved { get; set; }

        public UnloadedPickup (Pickup pickup)
        {
            Position = pickup.Position;
            PickupSerial = pickup.Serial;
            UnloadTime = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            HasBeenRemoved = false;

            if (Pickup != null && Pickup.IsSpawned) {
                Pickup.Rigidbody.Sleep();
                Pickup.UnSpawn();
            };
        }
    }
}
