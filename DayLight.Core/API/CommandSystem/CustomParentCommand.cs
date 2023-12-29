using CommandSystem;
using Exiled.API.Extensions;
using Exiled.Permissions.Extensions;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DayLight.Core.API.CommandSystem;

public abstract class CustomParentCommand : CommandHandler, ICommand
{
    public abstract string Command { get; }
    public abstract string[] Aliases { get; }
    public abstract string Description { get; }
    
    protected virtual string Permission { get; } = "";
    
    
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        return arguments.Count != 0 && TryGetCommand(arguments.Array[arguments.Offset], out var command) ?
            command.Execute(new ArraySegment<string>(arguments.Array, arguments.Offset + 1, arguments.Count - 1),
                sender, out response) : ExecuteParent(arguments, sender, out response);

    }
    
    readonly protected Dictionary<string, CustomCommand> SubCommands = new((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    readonly protected Dictionary<string, string> SubCommandsAliases = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    public override sealed void LoadGeneratedCommands()
    {
        RegisterCommands();
    }
    protected abstract void RegisterCommands();
    
    public bool TryGetCommand(string query, out CustomCommand command)
    {
        if (SubCommandsAliases.TryGetValue(query, out var str))
            query = str;
        return SubCommands.TryGetValue(query, out command);
    }
    protected void RegisterCommand(CustomCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Command))
            throw new ArgumentException("Command text of " + command.GetType().Name + " cannot be null or whitespace!");
        SubCommands.Add(command.Command, command);
        if (command.Aliases == null)
            return;
        foreach (var alias in command.Aliases)
        {
            if (!string.IsNullOrWhiteSpace(alias))
                SubCommandsAliases.Add(alias, command.Command);
        }
    }
    protected virtual bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        response = SubCommands.Values.Where(x => sender.CheckPermission(x.Permission)).Aggregate("\nPlease enter a valid subcommand:", (current, command) => current + $"\n\n<color=yellow><b>- {command.Command} " +
                                                                                                                                                            $"</b></color>\n<color=white>{command.Description}</color>");
        return false;
    }
    public override void UnregisterCommand(ICommand command)
    {
        SubCommands.Remove(command.Command);
        if (command.Aliases == null)
            return;
        foreach (string alias in command.Aliases)
            SubCommandsAliases.Remove(alias);    }

}
