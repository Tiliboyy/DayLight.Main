using CommandSystem;
using System;

namespace DayLight.Core.API.CommandSystem;

public abstract class CustomParent : ParentCommand
{

    public override void LoadGeneratedCommands()
    {
        
    }
    protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response) => throw new NotImplementedException();
    public override string Command { get; }
    public override string[] Aliases { get; }
    public override string Description { get; }
}
