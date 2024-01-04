using Exiled.API.Features;
using HarmonyLib;
using MEC;
using System;
using System.Collections.Generic;
using Server = Exiled.Events.Handlers.Server;
using Player = Exiled.Events.Handlers.Player;
using Map = Exiled.Events.Handlers.Map;
using Warhead = Exiled.Events.Handlers.Warhead;
using Exiled.Events.Handlers.Internal;
using InventorySystem.Items.Pickups;

namespace TotalCleaner
{
    public class TotalCleaner : Plugin<Config.Config>
    {
        public override string Name => "TotalCleaner";
        public override string Author => "creepycats";
        public override Version Version => new Version(1, 0, 0);

        public static TotalCleaner Instance { get; set; }

        private handlers.playerHandler PlayerHandler;
        private handlers.serverHandler ServerHandler;

        public List<CoroutineHandle> CleanupCoroutines = new List<CoroutineHandle>();

        public override void OnEnabled()
        {
            Instance = this;
            Log.Info($"{Name} v{Version} - made for v13 by creepycats");

            PlayerHandler = new handlers.playerHandler();
            ServerHandler = new handlers.serverHandler();

            Server.RestartingRound += ServerHandler.DisableCoroutines;
            Server.RoundStarted += ServerHandler.RoundStarted;
            Server.RoundEnded += ServerHandler.RoundEnded;

            Player.SpawnedRagdoll += PlayerHandler.SpawnedRagdoll;

            ItemPickupBase.OnPickupAdded += ServerHandler.PickupAdded;

            Map.Decontaminating += ServerHandler.Decontaminating;

            Warhead.Detonated += ServerHandler.Detonated;

            Log.Info("Plugin Enabled!");
        }
        public override void OnDisabled()
        {
            Server.RestartingRound -= ServerHandler.DisableCoroutines;
            Server.RoundStarted -= ServerHandler.RoundStarted;
            Server.RoundEnded -= ServerHandler.RoundEnded;

            Player.SpawnedRagdoll -= PlayerHandler.SpawnedRagdoll;

            ItemPickupBase.OnPickupAdded -= ServerHandler.PickupAdded;

            Map.Decontaminating -= ServerHandler.Decontaminating;

            Warhead.Detonated -= ServerHandler.Detonated;

            ServerHandler.DisableCoroutines();

            Log.Info("Disabled Plugin Successfully");
        }
    }
}