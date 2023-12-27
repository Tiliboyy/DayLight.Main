using Exiled.API.Features;
using Neuron.Core;
using System;
using System.Reflection;

namespace DayLight.Core.API;

public class Logger
{
    public static void Info(object Info) => Log.Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {Info}", Discord.LogLevel.Info, ConsoleColor.Cyan);
    public static void Error(object Error) => Log.Send($"[{Assembly.GetCallingAssembly().GetName().Name}] {Error}", Discord.LogLevel.Error, ConsoleColor.DarkRed);
    public static void Debug(object Debug)
    {
        if(Globals.Get<Config>().Debug)
            Log.Send($"[{(object)Assembly.GetCallingAssembly().GetName().Name}] {Debug}", Discord.LogLevel.Debug, ConsoleColor.Green);
    }
    public static void Sent(object message, Discord.LogLevel level, ConsoleColor color = ConsoleColor.Gray)
    {
        Log.SendRaw("[" + level.ToString().ToUpper() + "] " + message, color);
    }

    public static void Warn(object Warning) => Log.Send($"[{(object)Assembly.GetCallingAssembly().GetName().Name}] {Warning}", Discord.LogLevel.Warn, ConsoleColor.Magenta);

}
