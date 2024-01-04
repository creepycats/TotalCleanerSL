using Exiled.API.Features;
using Exiled.API.Features.Core;
using Exiled.API.Features.Pickups;
using Exiled.API.Interfaces;
using System;
using UnityEngine;
using BasePickup = InventorySystem.Items.Pickups.ItemPickupBase;

namespace TotalCleaner.Types
{
    public class CleanableRagdoll
    {
        public Ragdoll Ragdoll { get { return Ragdoll.GetLast(Player.Get(UserId)); } }
        public string UserId { get; }
        public Vector3 Position { get; }
        public bool HasBeenRemoved { get; set; }
        public long OutOfRangeSince { get; set; }

        public CleanableRagdoll(Ragdoll ragdoll, Vector3 pos)
        {
            UserId = ragdoll.Owner.UserId;
            Position = pos;
            HasBeenRemoved = false;
            OutOfRangeSince = 0;
        }
    }
}
