using CommandSystem;
using Exiled.API.Features;
using PluginAPI.Core;
using System;
using System.Globalization;
using Player = Exiled.API.Features.Player;

namespace DayLight.GameStore.Commands.ClientConsole;

[CommandHandler(typeof(ClientCommandHandler))]
internal class Balance : ICommand
{
    public string Command { get; } = "balance";

    public string[] Aliases { get; } = { "bal" };

    public string Description { get; } = "Zeigt dir deinen Kontostand an";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var player = Player.Get(sender);
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