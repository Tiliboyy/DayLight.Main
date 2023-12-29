using CommandSystem;
using DayLight.Core.API.Attributes;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RemoteAdmin;
using System;
using System.Linq;
using System.Reflection;

namespace DayLight.Core.API.CommandSystem;

public abstract class CustomCommand : ICommand
{



    public abstract string Command { get; }
    public abstract string[] Aliases { get; }
    public abstract string Description { get; }
    public virtual string Permission { get; } = "";
    protected virtual bool OnlyPlayers { get; } = true;

    protected abstract void Respond(ArraySegment<string> arguments, Player player, ref CommandResult response);


    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        try
        {
            if (!Player.TryGet(sender, out var player) || player.IsHost && OnlyPlayers)
            {
                response = "Error you can only execute this as a Player";
                return false;
            }
            if (Permission == "" || sender.CheckPermission(Permission))
            {

                var result = new CommandResult(); 
                Respond(arguments, player, ref result);
                response = result.Response;
                return result.Success;

            }
            response = $"Du hast nicht die erforderlichen Rechte. ({Permission})";
            return false;
        }
        catch (Exception ex)
        {
            response = "Error while executing the Command\n" +
                       $"Error while executing command {Command}\n{ex.Message}";
            Logger.Error(ex);
            return false;
        }
    }
    internal static void RegisterCommand(Type type)
    {
        var instance = Activator.CreateInstance(type);
        var customCommand = (ICommand)instance;
        var platforms = type.GetCustomAttribute<CommandAttribute>().Platforms;


        if (platforms.Contains(Platform.RemoteAdmin))
        {
            if (CommandProcessor.RemoteAdminCommandHandler.AllCommands.Any(x => x.Command == customCommand.Command))
            {
                Logger.Error($"The command {customCommand.Command} was already registered in the RemoteAdmin commands");
                return;
            }
            Logger.Debug($"Registering {customCommand.Command} as remoteadmin command");

            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(customCommand);

        }
        if (platforms.Contains(Platform.ServerConsole))
        {
            if (CommandProcessor.RemoteAdminCommandHandler.AllCommands.Any(x => x.Command == customCommand.Command))
            {
                Logger.Error($"The command {customCommand.Command} was already registered Console commands");
                return;
            }
            Logger.Debug($"Registering {customCommand.Command} as server console command");

            GameCore.Console.singleton.ConsoleCommandHandler.RegisterCommand(customCommand);

        }
        if (platforms.Contains(Platform.ClientConsole))
        {
            if (CommandProcessor.RemoteAdminCommandHandler.AllCommands.Any(x => x.Command == customCommand.Command))
            {
                Logger.Error($"The command {customCommand.Command} was already registered Client commands");
                return;
            }
            Logger.Debug($"Registering {customCommand.Command} as client console command");

            QueryProcessor.DotCommandHandler.RegisterCommand(customCommand);

        }
    }
}