using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Server;
using InventorySystem.Items.Pickups;
using MapEditorReborn.Commands.UtilityCommands;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using TotalCleaner.Classes;
using TotalCleaner.Types;

namespace TotalCleaner.handlers
{
    public class serverHandler
    {
        /// <summary>
        /// Disables Coroutines Running for TotalCleaner
        /// </summary>
        public void DisableCoroutines()
        {
            if (TotalCleaner.Instance.CleanupCoroutines.Count > 0)
            {
                TotalCleaner.Instance.CleanupCoroutines.ForEach(v => Timing.KillCoroutines(v));
                TotalCleaner.Instance.CleanupCoroutines.Clear();
            }

            AmmoCleaner.PlayerSpawnedAmmo.Clear();
            ObjectLoader.LoadedItems.Clear();
            ObjectLoader.UnloadedItems.Clear();
            ObjectLoader.RagdollQueue.Clear();

            ObjectLoader.RemovedItemCount = 0;
            ObjectLoader.RemovedRagdollCount = 0;

            ObjectLoader.ShowDataHintToPlayer = new List<string>();
        }

        /// <summary>
        /// Enables Coroutines During Round
        /// </summary>
        public void RoundStarted()
        {
            DisableCoroutines();

            if (TotalCleaner.Instance.Config.EnableAmmoClumping)
                TotalCleaner.Instance.CleanupCoroutines.Add(Timing.RunCoroutine(AmmoCleaner.HandleClumping()));

            TotalCleaner.Instance.CleanupCoroutines.Add(Timing.RunCoroutine(ObjectLoader.HandleLoading()));

            if (TotalCleaner.Instance.Config.TotalCleanerUI)
                TotalCleaner.Instance.CleanupCoroutines.Add(Timing.RunCoroutine(ObjectLoader.HandleDataHint()));
        }

        /// <summary>
        /// Disables Coroutines Post-Round
        /// </summary>
        public void RoundEnded(RoundEndedEventArgs args)
        {
            DisableCoroutines();
        }

        public void PickupAdded(ItemPickupBase ipb)
        {
            Pickup pkup = Pickup.Get(ipb);

            if (pkup.PreviousOwner == null) return;

            if (TotalCleaner.Instance.Config.PreservedItemsList.Contains(pkup.Type)) return;

            if (ObjectLoader.LoadedItems.Where(v=>v.Pickup.Base == ipb).Count() > 0) return; // Skip Re-loaded Items

            if (pkup is AmmoPickup ampkup && TotalCleaner.Instance.Config.EnableAmmoClumping) AmmoCleaner.PlayerSpawnedAmmo.Add(new CleanablePickup(ampkup)); // Do Ammo Clumping

            if (!TotalCleaner.Instance.Config.EnableItemLoading) return;

            ObjectLoader.LoadedItems.Add(new CleanablePickup(pkup));
        }

        public void Decontaminating(DecontaminatingEventArgs args)
        {
            List<Pickup> DeletePickups = Pickup.List.Where(pkup => pkup != null && pkup.Room != null && pkup.Room.Zone == ZoneType.LightContainment)?.ToList();
            List<Ragdoll> DeleteRagdolls = Ragdoll.List.Where(rgdl => rgdl != null && rgdl.Room != null && rgdl.Room.Zone == ZoneType.LightContainment)?.ToList();

            ObjectLoader.RemovedItemCount += DeletePickups.Count;
            ObjectLoader.RemovedRagdollCount += DeleteRagdolls.Count;

            foreach (Pickup pickup in DeletePickups)
            {
                try { pickup.Destroy(); } catch (Exception e) { }
            }
            foreach (Ragdoll ragdoll in DeleteRagdolls)
            {
                try { ragdoll.Destroy(); } catch (Exception e) { }
            }
            ObjectLoader.BroadcastClean(DeletePickups.Count, DeleteRagdolls.Count);
        }

        public void Detonated()
        {
            List<Pickup> DeletePickups = Pickup.List.Where(pkup => pkup != null && pkup.Room != null && pkup.Room.Zone != ZoneType.Surface).ToList();
            List<Ragdoll> DeleteRagdolls = Ragdoll.List.Where(rgdl => rgdl != null && rgdl.Room != null && rgdl.Room.Zone != ZoneType.Surface).ToList();

            ObjectLoader.RemovedItemCount += DeletePickups.Count;
            ObjectLoader.RemovedRagdollCount += DeleteRagdolls.Count;

            foreach (Pickup pickup in DeletePickups)
            {
                try { pickup.Destroy(); } catch (Exception e) { }
            }
            foreach (Ragdoll ragdoll in DeleteRagdolls)
            {
                try { ragdoll.Destroy(); } catch (Exception e) { }
            }
            ObjectLoader.BroadcastClean(DeletePickups.Count, DeleteRagdolls.Count);
        }
    }
}
