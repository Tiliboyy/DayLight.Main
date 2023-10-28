#region

using Exiled.API.Interfaces;
using Neuron.Core.Meta;
using Syml;
using System.Collections.Generic;
using System.ComponentModel;
using IConfig = DayLight.Core.IConfig;
using Vector3 = UnityEngine.Vector3;

#endregion

namespace DayLight.Moderation;

[Automatic]
[DocumentSection("ModerationSystem")]
public class ModerationConfig : IConfig
{
    public bool Enabled { get; set; } = true;
    public bool Debug { get; set; } = false;
    public string Broadcasttext { get; set; } =
        "<size=80%>Du wurdest <color=#fc1505><b>Verwarnt</b></color>\nNutze .gwarns für mehr Informationen\n\n\n</size>";

    public ushort Broadcasttexttime { get; set; } = 60;


    [Description("ID: 0 Is Default do not change")]
    public Dictionary<int, Vector3> Towers { get; set; } = new Dictionary<int, Vector3>()
    {
        {0, new Vector3(0,0,0)},
        {1, new Vector3(-15f,1014.5f,-31.4f)},
        {2, new Vector3(108,1014.01f,-13.7f)},
    };


}