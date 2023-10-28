using System;

namespace DayLight.Core.API.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute : Attribute
{
    public CommandAttribute(Platform[] platforms)
    {
        Platforms = platforms;
    }
    public Platform[] Platforms { get; set; }

}
public enum Platform
{
    ClientConsole,
    RemoteAdmin,
    ServerConsole
}