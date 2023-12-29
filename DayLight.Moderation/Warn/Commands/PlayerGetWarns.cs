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

    protected override bool Respond(ArraySegment<string> arguments, Player player, out string response)
    {

        var warns = WarnDatabase.GetWarns(player.UserId, true, out bool e);
        if (e == false)
        {
            response = "Du hast keine Verwarnungen";
            return true;
        }
        response = warns;
        return true;
    }
}