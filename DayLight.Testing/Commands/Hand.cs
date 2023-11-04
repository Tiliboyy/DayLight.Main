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

namespace DayLight.Test.Commands
{
  [Automatic]

  [Command(new [] { Platform.RemoteAdmin })]
  internal class Hand : CustomCommand
  {


    public override string Command { get; } = "Hand";
    public override string[] Aliases { get; } = Array.Empty<string>();
    public override string Description { get; } = "Hand someone!";
    protected override string Permission { get; } = "Test.Hand";
    protected override bool Respond(ArraySegment<string> arguments, Player player, out string response)
    {

        if (arguments.Count == 2)
        {
          if (arguments.Array != null && arguments.Array[1] == "*")
          {
            float.TryParse(arguments.Array[2], out var result);
            foreach (Player ply in (IEnumerable<Player>) Player.List)
              Timing.RunCoroutine(UnityMethods.HandCoroutine(ply, result));
            response = "Handed everyone";
            return true;
          }
          Player player1 = Player.Get(arguments.Array[1]);
          if (player1 == null)
          {
            response = "Player not found";
            return false;
          }
          float.TryParse(arguments.Array[2], out var result1);
          Timing.RunCoroutine(UnityMethods.HandCoroutine(player1, result1));
          response = "Handed " + player1.DisplayNickname;
          return true;
        }
        response = "Usage: Hand <ID> <Amount>";
        return false;
    }
  }
}
