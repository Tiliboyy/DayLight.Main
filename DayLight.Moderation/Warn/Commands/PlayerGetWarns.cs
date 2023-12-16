﻿#nullable enable

#region

using CommandSystem;
using Exiled.API.Features;
using System;

#endregion

namespace DayLight.Moderation.Warn.Commands;

[CommandHandler(typeof(ClientCommandHandler))]
public class Getwarnsplayer : ICommand
{
    public string Command { get; } = "getwarns";

    public string[] Aliases { get; } = new[]
    {
        "gwarns", "showwarns"
    };

    public string Description { get; } = "Usage: .getwarns";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender);
        if (player == null)
        {
            response = "You can only execute this as a player.";
            return true;
        }

        string str = WarnDatabase.GetWarns(player.UserId, true, out bool e);
        if (e == false)
        {
            response = "Du hast keine Verwarnungen";
            return true;
        }
        response = str;
        return true;
    }
}