// Decompiled with JetBrains decompiler
// Type: TestServerTools.Commands.UnityMethods
// Assembly: TestServerTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 005F6980-384A-4BBA-998F-1DCC627C1B82
// Assembly location: C:\Users\tilau\Desktop\TestServerTools.dll

using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using System.Collections.Generic;

namespace DayLight.Test.Commands
{
  public static class UnityMethods
  {
    public static IEnumerator<float> HandCoroutine(Player player, float time)
    {
      for (int i = 0; (double) i < (double) time; ++i)
      {
        player.EnableEffect((EffectType) 24, 0.0f, false);
        yield return Timing.WaitForSeconds(0.05f);
        player.DisableEffect((EffectType) 24);
        yield return Timing.WaitForSeconds(0.05f);
      }
    }
  }
}
