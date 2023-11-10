using PlayerRoles;
using System;
using System.Collections.Generic;

namespace DayLight.Core.Models;

[Serializable]
public struct Category
{
    public int id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
    public List<RoleTypeId> AllowedRoles { get; set; }
    public List<GameStoreItemPrice> Items { get; set; }
}
