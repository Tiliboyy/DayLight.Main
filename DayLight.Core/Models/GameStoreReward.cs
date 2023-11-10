using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Core.Models;

[Serializable]
public struct GameStoreReward
{
    public string Name { get; set; }
    public Dictionary<RoleTypeId, int> Money { get; set; }
    public int MaxPerRound { get; set; }
}

