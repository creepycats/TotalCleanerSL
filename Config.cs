using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Interfaces;
using Exiled.Events.Commands.PluginManager;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using TotalCleaner.Types;

namespace TotalCleaner.Config
{
    public class Config : IConfig
    {
        /// <summary>
        ///  Will the plugin run?
        /// </summary>
        public bool IsEnabled { get; set; } = true;
        /// <summary>
        ///  Will the plugin print Debug Text?
        /// </summary>
        public bool Debug { get; set; } = false;

        /// <summary>
        ///  Enable TotalCleanerUI Command?
        /// </summary>
        [Description("Enable TotalCleanerUI command?")]
        public bool TotalCleanerUI { get; set; } = true;
        /// <summary>
        ///  Should the TotalCleanerUI Command require the permission totalcleaner.ui?
        /// </summary>
        [Description("Should the TotalCleanerUI Command require the permission (totalcleaner.ui)?")]
        public bool UINeedsPermission { get; set; } = false;
        /// <summary>
        ///  How long should the game wait (in seconds) between checks for loading and unloading items and ragdolls
        /// </summary>
        [Description("How long should the game wait (in seconds) between checks for loading and unloading items and ragdolls")]
        public float LoadCheckInterval { get; set; } = 1.25f;
        /// <summary>
        ///  Enable Ammo Clumping? (Ammo Pickups will Combine when Close to Each Other)
        /// </summary>
        [Description("Enable Ammo Clumping? (Ammo Pickups will Combine when Close to Each Other)")]
        public bool EnableAmmoClumping { get; set; } = true;
        /// <summary>
        ///  How Close should Two Ammo Pickups be before they combine?
        /// </summary>
        [Description("How Close should Two Ammo Pickups be before they combine?")]
        public float AmmoClumpRange { get; set; } = 2f;
        /// <summary>
        ///  Enable Item Loading? (Despawns items far away from players and deletes them after enough time of being left alone)
        /// </summary>
        [Description("Enable Item Loading? (Despawns items far away from players and deletes them after enough time of being left alone)")]
        public bool EnableItemLoading { get; set; } = true;
        /// <summary>
        ///  How Close Should Items be to the Player in order to be Loaded?
        /// </summary>
        [Description("How Close Should Items be to the Player in order to be Loaded?")]
        public float RenderDistanceItem { get; set; } = 20f;
        /// <summary>
        ///  How long must an item be unrendered to be deleted?
        /// </summary>
        [Description("How many seconds until an unloaded item gets deleted from lack of players loading it?")]
        public float DestroyItemAfter { get; set; } = 50f;
        /// <summary>
        ///  What items should not be deleted when unloaded?
        /// </summary>
        [Description("What items should not be deleted when unloaded?")]
        public List<ItemType> PreservedItemsList { get; set; } = new List<ItemType>() {
            ItemType.SCP018, 
            ItemType.SCP1576, 
            ItemType.SCP1853, 
            ItemType.SCP207, 
            ItemType.SCP2176, 
            ItemType.SCP244a, 
            ItemType.SCP244b, 
            ItemType.SCP268, 
            ItemType.SCP330, 
            ItemType.SCP500, 
            ItemType.AntiSCP207,
            ItemType.Jailbird,
            ItemType.MicroHID,
            ItemType.ParticleDisruptor,
            ItemType.GrenadeHE,
            ItemType.GunCom45,
            ItemType.KeycardFacilityManager,
            ItemType.KeycardO5
        };
        /// <summary>
        ///  Enable Ragdoll Despawning? (Despawns items far away from players and deletes them after enough time of being left alone)
        /// </summary>
        [Description("Enable Ragdoll Despawning? (Despawns items far away from players and deletes them after enough time of being left alone)")]
        public bool EnableRagdollCleanup { get; set; } = true;
        /// <summary>
        ///  How Close Should Ragdolls be to the Player in order to be Preserved? Set to 0 to Delete Regardless of players nearby
        /// </summary>
        [Description("How Close Should Ragdolls be to the Player in order to be Preserved? Set to 0 to Delete Regardless of players nearby")]
        public float RenderDistanceRagdoll { get; set; } = 20f;
        /// <summary>
        ///  How many seconds until an unloaded ragdoll gets deleted?
        /// </summary>
        [Description("How many seconds until an unloaded ragdoll gets deleted?")]
        public float DestroyRagdollAfter { get; set; } = 50f;
        /// <summary>
        ///  How many ragdolls can be loaded at once? (Is independent of EnableRagdollDespawning)
        /// </summary>
        [Description("How many ragdolls can be loaded at once? (Is independent of EnableRagdollDespawning)")]
        public int MaxRagdolls { get; set; } = 10;
        /// <summary>
        ///  What roles should not have Ragdolls be deleted?
        /// </summary>
        [Description("What roles should not have Ragdolls be deleted?")]
        public List<RoleTypeId> PreservedRolesList { get; set; } = new List<RoleTypeId>() { 
            RoleTypeId.Scp0492,
            RoleTypeId.Flamingo,
            RoleTypeId.AlphaFlamingo
        };
        /// <summary>
        ///  Should bodies taken by SCP-3114 be kept?
        /// </summary>
        [Description("Should bodies taken by SCP-3114 be kept?")]
        public bool KeepSkeletonRagdolls { get; set; } = true;
        /// <summary>
        ///  Modify/Disable the Broadcast Messages from Large-Scale Cleanings here.
        /// </summary>
        [Description("Modify/Disable the Broadcast Messages from Large-Scale Cleanings here.")]
        public BroadcastConfig BroadcastConf { get; set; } = new BroadcastConfig();
    }
}
