using CommandSystem;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Neuron.Core.Meta;
using PluginAPI.Core;
using System;
using System.Globalization;
using Player = Exiled.API.Features.Player;

namespace DayLight.GameStore.Commands.ClientConsole;

[Automatic]
[Command(new [] { Platform.ClientConsole })]
internal class Balance : CustomCommand
{
    public override string Command { get; } = "balance";

    public override string[] Aliases { get; } = { "bal" };

    public override string Description { get; } = "Zeigt dir deinen Kontostand an";

    protected override bool Respond(ArraySegment<string> arguments, Player player, out string response)
    {
        if (player == null)
        {
            response = "You can only execute this as a Player";
            return false;
        }

        if (player.DoNotTrack)
        {
            response = GameStorePlugin.Instance.Translation.DntMessage;
            return true;
        }
        
        var balance = player.GetMoney();
        response = GameStorePlugin.Instance.Translation.BalanceMessage.Replace("(balance)", balance.ToString(CultureInfo.InvariantCulture));
        return true;
    }
}