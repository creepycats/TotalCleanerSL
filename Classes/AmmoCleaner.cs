using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using MapEditorReborn.API.Features.Objects;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalCleaner.Types;
using UnityEngine;

namespace TotalCleaner.Classes
{
    public class AmmoCleaner
    {
        public static List<CleanablePickup> PlayerSpawnedAmmo = new List<CleanablePickup>();

        public static IEnumerator<float> HandleClumping()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(0.5f);

                PlayerSpawnedAmmo = PlayerSpawnedAmmo.Where(pkup => !pkup.HasBeenRemoved && pkup.Pickup != null && pkup.Pickup.IsSpawned).ToList();

                // Clump Logic
                foreach (CleanablePickup ammoPickup in PlayerSpawnedAmmo)
                {
                    if (ammoPickup.HasBeenRemoved) continue;

                    if (ammoPickup.Pickup is not AmmoPickup apkup) continue;

                    foreach (CleanablePickup comparePickup in PlayerSpawnedAmmo.Where(pkup => !pkup.HasBeenRemoved && pkup.Pickup.Base != ammoPickup.Pickup.Base))
                    {
                        if (comparePickup.Pickup is not AmmoPickup cpkup) continue;

                        if (apkup.AmmoType != cpkup.AmmoType) continue;

                        if (Vector3.Distance(ammoPickup.Pickup.Position, comparePickup.Pickup.Position) < 1f)
                        {
                            comparePickup.HasBeenRemoved = true;

                            ammoPickup.Pickup.Position = Vector3.Lerp(ammoPickup.Pickup.Position, comparePickup.Pickup.Position, 0.5f);
                            ammoPickup.Pickup.Rotation = Quaternion.Lerp(ammoPickup.Pickup.Rotation, comparePickup.Pickup.Rotation, 0.5f);

                            apkup.Ammo = (ushort)(apkup.Ammo + cpkup.Ammo);

                            comparePickup.Pickup.Destroy();
                        } 
                    }
                }
            }
        }
    }
}
