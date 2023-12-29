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

namespace DayLight.Test.Commands;

[Automatic]

[Command(new [] { Platform.RemoteAdmin })]
internal class Shit : CustomCommand
{
  public override string Command { get; } = "shit";

  public override string[] Aliases { get; } = Array.Empty<string>();

  public override string Description { get; } = "shit";

  public override string Permission { get; } = "Test.Fun";

  protected override void Respond(ArraySegment<string> arguments, Player sender, ref CommandResult commandResult)
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
          commandResult.Response = "Shat everyone";
          commandResult.Success = true;
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
        Timing.RunCoroutine(ShitPlayer(player1, result1));
        commandResult.Response = "Shit " + player1.DisplayNickname;
        commandResult.Success = true;
        return;
      }
      commandResult.Response = "Usage: Shit <ID> <Amount>";
      commandResult.Success = false;
      return;
      
    }
    commandResult.Response = "Usage: Shit <ID> <Amount>";
    commandResult.Success = false;
    
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