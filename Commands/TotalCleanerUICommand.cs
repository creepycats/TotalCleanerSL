namespace SCPCosmetics.Commands.Pet
{
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using PlayerRoles;
    using RemoteAdmin;
    using System;
    using System.Linq;
    using TotalCleaner;
    using TotalCleaner.Classes;
    using UnityEngine;

    [CommandHandler(typeof(ClientCommandHandler))]
    public class TotalCleanerUICommand : ICommand
    {
        public string Command => "tcleanui";
        public string[] Aliases { get; } = { "tcui" };
        public string Description => "Use to Toggle Displaying TotalCleaner Hint UI";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not PlayerCommandSender)
            {
                response = "This command can only be ran by a player!";
                return true;
            }

            if (!TotalCleaner.Instance.Config.TotalCleanerUI)
            {
                response = "[TotalCleaner] - TotalCleanerUI is disabled on this server";
                return false;
            }

            if (TotalCleaner.Instance.Config.UINeedsPermission && !sender.CheckPermission("totalcleaner.ui"))
            {
                response = "[TotalCleaner] - You lack the permissions required for TotalCleanerUI";
                return false;
            }

            Player player = Player.Get(((PlayerCommandSender)sender).ReferenceHub);
            if (ObjectLoader.ShowDataHintToPlayer.Contains(player.UserId))
            {
                ObjectLoader.ShowDataHintToPlayer.Remove(player.UserId);
                response = $"[TotalCleaner] - Disabled Custom TotalCleaner Status UI";
                return true;
            }

            ObjectLoader.ShowDataHintToPlayer.Add(player.UserId);
            response = $"[TotalCleaner] - Enabled Custom TotalCleaner Status UI";
            return true;
        }
    }
}
