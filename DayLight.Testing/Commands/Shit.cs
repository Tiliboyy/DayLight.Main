// Decompiled with JetBrains decompiler
// Type: TestServerTools.Commands.Shit
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
  internal class Shit : CustomCommand
  {
    public override string Command { get; } = "shit";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "shit";

    protected override string Permission { get; } = "Test.Fun";

    protected override bool Respond(ArraySegment<string> arguments, Player sender, out string response)
    {

        if (arguments.Count == 2)
        {
          if (arguments.Array != null)
          {
            if (arguments.Array[1] == "*")
            {
              float.TryParse(arguments.Array[2], out var result);
              foreach (Player player in (IEnumerable<Player>) Player.List)
                Timing.RunCoroutine(ShitPlayer(player, result));
              response = "Shat everyone";
              return true;
            }
            Player player1 = Player.Get(arguments.Array[1]);
            if (player1 == null)
            {
              response = "Player not found";
              return false;
            }
            float.TryParse(arguments.Array[2], out var result1);
            Timing.RunCoroutine(ShitPlayer(player1, result1));
            response = "Shit " + player1.DisplayNickname;
            return true;
          }
          response = "Usage: Shit <ID> <Amount>";
          return false;
        }
        response = "Usage: Shit <ID> <Amount>";
        return false;
    }

    public IEnumerator<float> ShitPlayer(Player player, float amount)
    {
      for (int i = 0; (double) i < (double) amount - 1.0; ++i)
      {
        Map.PlaceTantrum(player.Position, true);
        yield return Timing.WaitForSeconds(0.1f);
      }
      yield return 1f;
    }
  }
}
