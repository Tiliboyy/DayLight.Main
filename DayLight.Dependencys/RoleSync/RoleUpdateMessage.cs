using System;
using System.Collections.Generic;

namespace DayLight.Dependencys.RoleSync;

[Serializable]
public struct RoleUpdater
{
    public ulong UserID { get; set; }

    public ulong RoleID { get; set; }
    public string RoleName { get; set; }

    public List<ulong> Overrideables { get; set; }
}
