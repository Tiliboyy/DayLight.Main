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
    protected virtual string Permission { get; } = "";

    protected abstract bool Respond(ArraySegment<string> arguments, Player player, out string response);
  
    
    
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        try
        {
            if (!Player.TryGet(sender, out var player) || player.IsHost)
            {
                response = "Knecht";
                return false;
            }
            if (Permission == "" || sender.CheckPermission(Permission)) return Respond(arguments, player, out response);
            response = $"Du hast nicht die erforderlichen Rechte. ({Permission})";
            return true;
        }
        catch (Exception ex)
        {
            response = "Error while executing the Command\n" +
                       $"{ex.Message}";
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
            }
            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(customCommand);

        }
        if (platforms.Contains(Platform.ServerConsole))
        {
            if (CommandProcessor.RemoteAdminCommandHandler.AllCommands.Any(x => x.Command == customCommand.Command))
            {
                Logger.Error($"The command {customCommand.Command} was already registered Console commands");
            }
            GameCore.Console.singleton.ConsoleCommandHandler.RegisterCommand(customCommand);

        }
        if (platforms.Contains(Platform.ClientConsole))
        {
            if (CommandProcessor.RemoteAdminCommandHandler.AllCommands.Any(x => x.Command == customCommand.Command))
            {
                Logger.Error($"The command {customCommand.Command} was already registered Client commands");
            }
            QueryProcessor.DotCommandHandler.RegisterCommand(customCommand);

        }
    }
}
