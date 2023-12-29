#nullable enable

#region

using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Neuron.Core.Meta;
using System;

#endregion

namespace DayLight.Moderation.Warn.Commands;

[Automatic]
[Command(new [] { Platform.ClientConsole })]
public class Getwarnsplayer : CustomCommand
{
    public override string Command { get; } = "showwarns";

    public override string[] Aliases { get; } = new[]
    {
        "mywarns", "swarns"
    };

    public override string Description { get; } = "Usage: .getwarns";

    protected override void Respond(ArraySegment<string> arguments, Player sender, ref CommandResult response)
    {

        var warns = WarnDatabase.GetWarns(sender.UserId, true, out bool e);
        if (e == false)
        {
            response.Response = "Du hast keine Verwarnungen";
            response.Success = true;
            return;
        }
        response.Response = warns;
        response.Success = true;
    }
}