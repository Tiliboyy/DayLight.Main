using Exiled.API.Features;
using Mono.Security.X509;
using System;
using System.Reflection;

namespace DayLight.Core;

public class Logger
{
    public static void Info(object Info) => Log.Info(Info);
    public static void Error(object Error) => Log.Error(Error);
    public static void Debug(object Debug)
    {
        var callingAssembly = Assembly.GetCallingAssembly();
        Log.Send($"[{(object)callingAssembly.GetName().Name}] {Debug}", Discord.LogLevel.Debug, ConsoleColor.Green);
    }  

    public static void Warn(object Warning) => Log.Warn(Warning);

}
