// Decompiled with JetBrains decompiler
// Type: TestServerTools.Commands.Hand
// Assembly: TestServerTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 005F6980-384A-4BBA-998F-1DCC627C1B82
// Assembly location: C:\Users\tilau\Desktop\TestServerTools.dll

using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using MEC;
using Neuron.Core.Meta;
using System;
using System.Collections.Generic;

namespace DayLight.Test.Commands;

[Automatic]

[Command(new [] { Platform.RemoteAdmin })]
internal class Hand : CustomCommand
{


  public override string Command { get; } = "Hand";
  public override string[] Aliases { get; } = Array.Empty<string>();
  public override string Description { get; } = "Hand someone!";
  public override string Permission { get; } = "Test.Hand";
  protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult commandResult)
  {

    if (arguments.Count == 2)
    {
      if (arguments.Array != null && arguments.Array[1] == "*")
      {
        float.TryParse(arguments.Array[2], out var result);
        foreach (Player ply in (IEnumerable<Player>) Player.List)
          Timing.RunCoroutine(UnityMethods.HandCoroutine(ply, result));
        commandResult.Response = "Handed everyone";
        return;
      }
      Player player1 = Player.Get(arguments.Array[1]);
      if (player1 == null)
      {
        commandResult.Response = "Player not found";
        commandResult.Success = false;
        return;
      }
      float.TryParse(arguments.Array[2], out var result1);
      Timing.RunCoroutine(UnityMethods.HandCoroutine(player1, result1));
      commandResult.Response = "Handed " + player1.DisplayNickname;
      commandResult.Success = false;

      return;
    }
    commandResult.Response = "Usage: Hand <ID> <Amount>";
    commandResult.Success = false;
    return;
  }
}