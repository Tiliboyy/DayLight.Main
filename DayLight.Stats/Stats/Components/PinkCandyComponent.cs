#region

using UnityEngine;

#endregion

namespace DayLight.Stats.Stats.Components;

public class PinkCandyComponent : MonoBehaviour
{
    public void DestroyComponent()
    {
        DestroyImmediate(this);
    }
}
