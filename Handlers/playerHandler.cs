using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using System.Linq;
using UnityEngine;
using Exiled.Events.Handlers.Internal;
using TotalCleaner.Classes;
using TotalCleaner.Types;
using Exiled.Events.EventArgs.Scp049;
using System.Collections.Generic;
using MEC;
using PlayerRoles.PlayableScps.Scp3114;
using System;

namespace TotalCleaner.handlers
{
    public class playerHandler
    {
        public void SpawnedRagdoll(SpawnedRagdollEventArgs args)
        {
            if (args.Player == null || args.Ragdoll == null || args.Player.UserId == null) return;

            if (TotalCleaner.Instance.Config.PreservedRolesList.Contains(args.Ragdoll.Role)) return; // Skip Preserved Roles'

            if (TotalCleaner.Instance.Config.KeepSkeletonRagdolls && args.Ragdoll.DamageHandler is Scp3114DamageHandler hndlr && hndlr.Subtype == Scp3114DamageHandler.HandlerType.SkinSteal) return;

            if (Ragdoll.Get(args.Player).Where(rgdl => rgdl != args.Ragdoll).Count() > 0 && Server.PlayerCount > 1) {
                foreach (Ragdoll rgdl in Ragdoll.Get(args.Player).Where(rgdl => rgdl != args.Ragdoll && !(TotalCleaner.Instance.Config.KeepSkeletonRagdolls && rgdl.DamageHandler is Scp3114DamageHandler hndlr && hndlr.Subtype == Scp3114DamageHandler.HandlerType.SkinSteal)))
                {
                    try { rgdl.Destroy(); } catch (Exception e) { }
                }

                ObjectLoader.RagdollQueue = new Queue<CleanableRagdoll>(ObjectLoader.RagdollQueue.Where(clrd => clrd.UserId != args.Player.UserId).ToList());
            }

            ObjectLoader.RagdollQueue.Enqueue(new CleanableRagdoll(args.Ragdoll, args.Position)); // Add to Queue
        }

        public void FinishingRecall(FinishingRecallEventArgs args) 
        {
            if (args.Player == null || args.Ragdoll == null) return;

            if (ObjectLoader.RagdollQueue.Where(rgdl => rgdl.Ragdoll == args.Ragdoll).Count() > 0)
            {
                ObjectLoader.RagdollQueue = new Queue<CleanableRagdoll>(ObjectLoader.RagdollQueue.Where(rgdl => rgdl.Ragdoll != args.Ragdoll).ToList());
            }
        }
    }
}
