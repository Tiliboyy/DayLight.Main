// Decompiled with JetBrains decompiler
// Type: TestServerTools.Commands.NoDroppedItems
// Assembly: TestServerTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 005F6980-384A-4BBA-998F-1DCC627C1B82
// Assembly location: C:\Users\tilau\Desktop\TestServerTools.dll

using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using MEC;
using Neuron.Core.Meta;
using System;
using System.Collections.Generic;

namespace DayLight.Test.Commands
{
  [Automatic]

  [Command(new [] { Platform.RemoteAdmin })]
  internal class NoDroppedItems : CustomCommand
  {
    public static bool NoItems;
    public CoroutineHandle CoroutineHandle;

    public override string Command { get; } = "NoDroppedItems";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = nameof (NoDroppedItems);

    protected override string Permission { get; } = "Test.Test";

    protected override bool Respond(ArraySegment<string> arguments, Player player, out string response) 
    {
      if (NoItems)
      {
        NoItems = false;
        Timing.KillCoroutines(new CoroutineHandle[1]
        {
          CoroutineHandle
        });
        response = "Disabled";
        return true;
      }
      CoroutineHandle = Timing.RunCoroutine(DeleteItems());
      NoItems = true;
      response = "Enabled";
      return true;
    }

    public IEnumerator<float> DeleteItems()
    {
      while (true)
      {
        foreach (Pickup item in Pickup.List)
          item.Destroy();
        yield return Timing.WaitForSeconds(0.2f);
      }
    }
  }
}
