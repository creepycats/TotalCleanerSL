using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using MapEditorReborn.API.Features.Objects;
using MEC;
using PlayerRoles.PlayableScps.Scp3114;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TotalCleaner.Types;
using UnityEngine;

namespace TotalCleaner.Classes
{
    public class ObjectLoader
    {
        public static List<CleanablePickup> LoadedItems = new List<CleanablePickup>();

        public static List<UnloadedPickup> UnloadedItems = new List<UnloadedPickup>();

        public static Queue<CleanableRagdoll> RagdollQueue = new Queue<CleanableRagdoll>();

        public static int RemovedItemCount = 0;
        public static int RemovedRagdollCount = 0;

        public static List<string> ShowDataHintToPlayer = new List<string>();

        public static IEnumerator<float> HandleLoading()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(TotalCleaner.Instance.Config.LoadCheckInterval);

                long CurrentTime = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                try
                {
                    List<Player> LCZ_Players = Player.List.Where(plr => !plr.IsDead && plr.CurrentRoom != null && plr.CurrentRoom.Zone == Exiled.API.Enums.ZoneType.LightContainment).ToList();
                    List<Player> HCZ_Players = Player.List.Where(plr => !plr.IsDead && plr.CurrentRoom != null && plr.CurrentRoom.Zone == Exiled.API.Enums.ZoneType.HeavyContainment).ToList();
                    List<Player> Entrance_Players = Player.List.Where(plr => !plr.IsDead && plr.CurrentRoom != null && plr.CurrentRoom.Zone == Exiled.API.Enums.ZoneType.Entrance).ToList();
                    List<Player> Surface_Players = Player.List.Where(plr => !plr.IsDead && plr.CurrentRoom != null && plr.CurrentRoom.Zone == Exiled.API.Enums.ZoneType.Surface).ToList();

                    // Unloader Logic
                    if (TotalCleaner.Instance.Config.EnableItemLoading)
                    {
                        LoadedItems = LoadedItems.Where(pkup => !pkup.HasBeenRemoved && pkup.Pickup != null && pkup.Pickup.IsSpawned).ToList();
                        UnloadedItems = UnloadedItems.Where(pkup => !pkup.HasBeenRemoved && pkup.Pickup != null && !pkup.Pickup.IsSpawned).ToList();

                        foreach (CleanablePickup clpkup in LoadedItems)
                        {
                            if (clpkup.HasBeenRemoved) continue;

                            bool isNear = false;
                            if (clpkup.Pickup.Room != null)
                            {
                                List<Player> CheckPlayers = new List<Player>();
                                switch (clpkup.Pickup.Room.Zone) {
                                    case ZoneType.LightContainment:
                                        CheckPlayers = LCZ_Players; break;
                                    case ZoneType.HeavyContainment:
                                        CheckPlayers = HCZ_Players; break;
                                    case ZoneType.Entrance:
                                        CheckPlayers = Entrance_Players; break;
                                    case ZoneType.Surface:
                                        CheckPlayers = Surface_Players; break;
                                }

                                foreach (Player player in CheckPlayers)
                                {
                                    if (isNear) continue;
                                    if (Vector3.Distance(player.Position, clpkup.Pickup.Position) < TotalCleaner.Instance.Config.RenderDistanceItem) isNear = true;
                                }
                            }

                            if (!isNear)
                            {
                                clpkup.HasBeenRemoved = true;
                                UnloadedItems.Add(new UnloadedPickup(clpkup.Pickup));
                            }
                        }
                        foreach (UnloadedPickup clpkup in UnloadedItems)
                        {
                            if (clpkup.HasBeenRemoved) continue;

                            bool isNear = false;
                            if (clpkup.Pickup.Room != null)
                            {
                                List<Player> CheckPlayers = new List<Player>();
                                switch (clpkup.Pickup.Room.Zone)
                                {
                                    case ZoneType.LightContainment:
                                        CheckPlayers = LCZ_Players; break;
                                    case ZoneType.HeavyContainment:
                                        CheckPlayers = HCZ_Players; break;
                                    case ZoneType.Entrance:
                                        CheckPlayers = Entrance_Players; break;
                                    case ZoneType.Surface:
                                        CheckPlayers = Surface_Players; break;
                                }

                                foreach (Player player in CheckPlayers)
                                {
                                    if (isNear) continue;
                                    if (Vector3.Distance(player.Position, clpkup.Position) < TotalCleaner.Instance.Config.RenderDistanceItem) isNear = true;
                                }
                            }

                            if (isNear)
                            {
                                clpkup.HasBeenRemoved = true;
                                LoadedItems.Add(new CleanablePickup(clpkup.Pickup));
                            }
                            else
                            {
                                if (CurrentTime - clpkup.UnloadTime > TotalCleaner.Instance.Config.DestroyItemAfter)
                                {
                                    clpkup.HasBeenRemoved = true;
                                    try { clpkup.Pickup.Destroy(); } catch (Exception e) { }
                                    RemovedItemCount++;
                                }
                            }
                        }
                    };

                    // Ragdoll Limit
                    while (RagdollQueue.Count() > TotalCleaner.Instance.Config.MaxRagdolls)
                    {
                        CleanableRagdoll rgdl = RagdollQueue.Dequeue();

                        if (rgdl.HasBeenRemoved) continue;

                        if (!(TotalCleaner.Instance.Config.KeepSkeletonRagdolls && rgdl.Ragdoll.DamageHandler is Scp3114DamageHandler hndlr && hndlr.Subtype == Scp3114DamageHandler.HandlerType.SkinSteal))
                        {
                            Timing.CallDelayed(2f, () =>
                            {
                                try { rgdl.Ragdoll.Destroy(); } catch (Exception e) { }
                                RemovedRagdollCount++;
                            });
                        }
                        rgdl.HasBeenRemoved = true;
                    }

                    RagdollQueue = new Queue<CleanableRagdoll>(RagdollQueue.Where(rgdl => !rgdl.HasBeenRemoved && rgdl.Ragdoll != null));

                    if (!TotalCleaner.Instance.Config.EnableRagdollCleanup) continue;

                    foreach (CleanableRagdoll rgdl in RagdollQueue)
                    {
                        if (rgdl.HasBeenRemoved) continue;
                        if (rgdl.Ragdoll == null) continue;

                        bool isNear = false;
                        if (rgdl.Ragdoll.Room != null)
                        {
                            List<Player> CheckPlayers = new List<Player>();
                            switch (rgdl.Ragdoll.Room.Zone)
                            {
                                case ZoneType.LightContainment:
                                    CheckPlayers = LCZ_Players; break;
                                case ZoneType.HeavyContainment:
                                    CheckPlayers = HCZ_Players; break;
                                case ZoneType.Entrance:
                                    CheckPlayers = Entrance_Players; break;
                                case ZoneType.Surface:
                                    CheckPlayers = Surface_Players; break;
                            }

                            foreach (Player player in CheckPlayers)
                            {
                                if (isNear) continue;
                                if (Vector3.Distance(player.Position, rgdl.Position) < TotalCleaner.Instance.Config.RenderDistanceRagdoll) isNear = true;
                            }
                        }

                        if (isNear)
                        {
                            rgdl.OutOfRangeSince = 0;
                        }
                        else
                        {
                            if (rgdl.OutOfRangeSince == 0) rgdl.OutOfRangeSince = CurrentTime;
                            else
                            {
                                if (CurrentTime - rgdl.OutOfRangeSince < TotalCleaner.Instance.Config.DestroyRagdollAfter) continue;

                                if (!(TotalCleaner.Instance.Config.KeepSkeletonRagdolls && rgdl.Ragdoll.DamageHandler is Scp3114DamageHandler hndlr && hndlr.Subtype == Scp3114DamageHandler.HandlerType.SkinSteal))
                                {
                                    try { rgdl.Ragdoll.Destroy(); } catch (Exception e) { }
                                    RemovedRagdollCount++;
                                }
                                rgdl.HasBeenRemoved = true;
                            }
                        }
                    }
                }
                catch (Exception e) { Log.Debug(e); }
                }
        }

        public static void BroadcastClean(int items, int ragdolls)
        {
            if(TotalCleaner.Instance.Config.BroadcastConf.CleanupInfoChannel != CleanupMsgChannel.None)
            {
                string msg = $"<size=25>{TotalCleaner.Instance.Config.BroadcastConf.CleanupInfoPrefix}</size>\n" + ParseCleanupMessage(TotalCleaner.Instance.Config.BroadcastConf.CleanupMessage, items, ragdolls);
                switch (TotalCleaner.Instance.Config.BroadcastConf.CleanupInfoChannel) {
                    case CleanupMsgChannel.Broadcast:
                        Map.Broadcast(new Exiled.API.Features.Broadcast(msg, 3, true), true);
                        break;
                    case CleanupMsgChannel.Hint:
                        foreach (Player plr in Player.List) {
                            plr.ShowHint(msg, 3);
                        }
                        break;
                }
            }
        }

        public static string ParseCleanupMessage(string msg, int items, int ragdolls)
        {
            return msg.Replace("{items}", items.ToString()).Replace("{ragdolls}", ragdolls.ToString());
        }

        public static IEnumerator<float> HandleDataHint()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(1f);

                ShowDataHintToPlayer = ShowDataHintToPlayer.Where(uid => Player.Get(uid) != null).ToList();
                foreach (string uid in ShowDataHintToPlayer)
                {
                    Player plr = Player.Get(uid);

                    if (plr.CurrentHint == null || plr.CurrentHint.Content.Contains(TotalCleaner.Instance.Config.BroadcastConf.CleanupInfoPrefix))
                    {
                        plr.ShowHint($"<size=555>\n</size><size=20>{TotalCleaner.Instance.Config.BroadcastConf.CleanupInfoPrefix}\n{Server.Tps.ToString(CultureInfo.InvariantCulture)}/{Application.targetFrameRate.ToString()} <color=#8f8f8f>Ticks Per Second</color></size><size=25>\n{LoadedItems.Count} <color=#8f8f8f><size=20>Loaded</size></color> : {UnloadedItems.Count} <color=#8f8f8f><size=20>Unloaded Items</size></color> | {RagdollQueue.Count()}/{TotalCleaner.Instance.Config.MaxRagdolls} <color=#8f8f8f><size=20>Ragdoll Queue</size></color>\n{RemovedItemCount} <color=#8f8f8f><size=20>Items</size></color> | <color=red>Removed</color> | {RemovedRagdollCount} <color=#8f8f8f><size=17>Ragdolls</size></color></size>", 1.1f);
                    } 
                }
            }
        }
    }
}
