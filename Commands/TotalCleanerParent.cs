namespace TotalCleaner.Commands
{
    using CommandSystem;
    using Exiled.Permissions.Extensions;
    using RemoteAdmin;
    using System;

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class TotalCleanerCommand : ParentCommand, IUsageProvider
    {
        public TotalCleanerCommand() => LoadGeneratedCommands();

        public override string Command { get; } = "totalcleaner";

        public override string[] Aliases { get; } = { "tclean", "totalclean" };

        public override string Description { get; } = "TotalCleaner Root Command - Use to Clean All Ragdolls and Items of Certain Types";

        public string[] Usage { get; } = { "Items/Ragdolls", "Light/Heavy/Entrance/Surface/All", "ItemType/RoleType (Optional, Can be Multiple)" };

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Cleanup.CleanItemCommand());
            RegisterCommand(new Cleanup.CleanRagdollCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not PlayerCommandSender)
            {
                response = "This command can only be ran by a player!";
                return true;
            }

            if (!sender.CheckPermission("totalcleaner.commands"))
            {
                response = "[TotalCleaner] - Failed : Lack of Permission to Perform this Command!";
                return false;
            }

            response = "[TotalCleaner] - Failed : Invalid Subcommand.";
            return false;
        }
    }
}
