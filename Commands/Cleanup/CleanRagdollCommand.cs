namespace TotalCleaner.Commands.Cleanup
{
    using CommandSystem;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.API.Features.Pickups;
    using Exiled.Permissions.Extensions;
    using global::TotalCleaner.Classes;
    using MapEditorReborn.Commands.UtilityCommands;
    using PlayerRoles;
    using RemoteAdmin;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CleanRagdollCommand : ICommand, IUsageProvider
    {
        public string Command => "ragdolls";

        public string[] Aliases => new string[]
        {
            "ragdoll"
        };

        public string Description => "Cleans up Ragdolls in a Given Zone";

        public string[] Usage { get; } = { "Light/Heavy/Entrance/Surface/All", "%role% (Optional)" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Player.TryGet(sender, out Player player))
            {
                response = "This command can only be ran by a player!";
                return true;
            }

            if (!sender.CheckPermission("totalcleaner.commands"))
            {
                response = "[TotalCleaner] - Failed : Lack of Permission to Perform this Command!";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "[TotalCleaner] - Failed : Missing Required Parameter 'Zone'";
                return false;
            }

            ZoneType clearzone = ZoneType.Unspecified;
            switch (arguments.At(0).ToLower())
            {
                case "light":
                case "l":
                case "lightcontainment":
                case "lightcontainmentzone":
                case "lightcont":
                case "lcz":
                case "lc":
                    clearzone = ZoneType.LightContainment;
                    break;
                case "heavy":
                case "h":
                case "heavycontainment":
                case "heavycontainmentzone":
                case "heavycont":
                case "hcz":
                case "hc":
                    clearzone = ZoneType.HeavyContainment;
                    break;
                case "entrance":
                case "entrancezone":
                case "ez":
                case "e":
                    clearzone = ZoneType.Entrance;
                    break;
                case "all":
                case "a":
                    clearzone = ZoneType.Other;
                    break;
                default:
                    response = "[TotalCleaner] - Failed : Invalid Required Parameter 'Zone'";
                    return false;
            }

            List<RoleTypeId> DeletedRoles = Enum.GetValues(typeof(RoleTypeId)).ToArray<RoleTypeId>().ToList();
            if (arguments.Count > 1)
            {
                DeletedRoles = new List<RoleTypeId>();
                foreach (var item in arguments.Skip(1))
                {
                    if (Enum.TryParse<RoleTypeId>(item, out RoleTypeId roleTypeOut)) DeletedRoles.Add(roleTypeOut);
                }
            }

            int numDeleted = 0;
            foreach(Ragdoll rgdl in Ragdoll.List)
            {
                if (rgdl == null) continue;
                if ((clearzone == ZoneType.Other || rgdl.Room.Zone == clearzone) && DeletedRoles.Contains(rgdl.Role)) {
                    rgdl.Destroy();
                    numDeleted++;
                }
            }

            ObjectLoader.BroadcastClean(0, numDeleted);
            response = $"[TotalCleaner] - Cleaned : {numDeleted} Ragdolls Removed from Zone {arguments.At(0)}";
            return true;
        }
    }
}
