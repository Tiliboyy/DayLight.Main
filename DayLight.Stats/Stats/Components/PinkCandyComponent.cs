#region

using UnityEngine;

#endregion

namespace DayLight.Stat.Stats.Components;

public class PinkCandyComponent : MonoBehaviour
{
    public void DestroyComponent()
    {
        DestroyImmediate(this);
    }
}
