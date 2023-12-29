using DayLight.Core.API;
using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Neuron.Core.Meta;
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

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult response)
    {

        if (player.DoNotTrack)
        {
            response.Response = GameStorePlugin.Instance.Translation.DntMessage;
            response.Success = false;
            return;
        }
        
        var balance = player.GetMoney();
        response.Response = GameStorePlugin.Instance.Translation.BalanceMessage.Replace("(balance)", balance.ToString(CultureInfo.InvariantCulture));
        return;
    }
}