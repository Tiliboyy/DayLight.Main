// Decompiled with JetBrains decompiler
// Type: TestServerTools.Commands.AutoSpawn
// Assembly: TestServerTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 005F6980-384A-4BBA-998F-1DCC627C1B82
// Assembly location: C:\Users\tilau\Desktop\TestServerTools.dll

using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Neuron.Core.Meta;
using System;
using System.Collections.Generic;

namespace DayLight.Test.Commands
{
  [Automatic]
  [Command(new [] { Platform.RemoteAdmin })]
  internal class AutoSpawn : CustomCommand
  {
    public static List<Player> RespawnPlayers = new();

    public override string Command { get; } = nameof (AutoSpawn);

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = nameof (AutoSpawn);

    protected override bool Respond(ArraySegment<string> arguments, Player player, out string response)
    {
      if (RespawnPlayers.Contains(player))
      {
        RespawnPlayers.Remove(player);
        response = "Removed";
        return true;
      }
      RespawnPlayers.Add(player);
      response = "Added";
      return true;
    }
  }
}
