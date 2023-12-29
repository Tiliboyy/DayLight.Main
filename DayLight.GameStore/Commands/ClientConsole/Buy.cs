using DayLight.Core.API.Attributes;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Neuron.Core.Meta;
using NorthwoodLib.Pools;
using System;

namespace DayLight.GameStore.Commands.ClientConsole;
[Automatic]
[Command(new [] { Platform.ClientConsole })]
internal class Buy : CustomCommand
{
    public override string Command { get; } = "buy";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "buy";

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult commandResult)
    {

        if (player.DoNotTrack)
        {
            commandResult.Response = GameStorePlugin.Instance.Translation.DntMessage;
            commandResult.Success = false;
            return;
        }
        if (!GameStorePlugin.EnableGamestore)
        {
            commandResult.Response = GameStorePlugin.Instance.Translation.DisabledStore;
            commandResult.Success = false;
            return;
        }

        switch (arguments.Count)
        {
            case 0:
                
                if (!player.IsAlive)
                {
                    commandResult.Response = player.GetAvailableCategories(true);
                    return;
                }
                commandResult.Response = player.GetAvailableCategories();
                return;
            case 1:
            {
                

                if (int.TryParse(arguments.Array[1], out var argument1))
                {
                    if (!player.IsAlive)
                    {
                        commandResult.Response = player.GetAvailableItems(argument1);
                        return;
                    }
                    commandResult.Response = player.GetAvailableItems(argument1);
                    return;
                }
                if (!player.IsAlive)
                {
                    commandResult.Response = GameStorePlugin.Instance.Translation.WrongeRole;
                    commandResult.Success = false;
                    return;
                }
                var name = FormatArguments(arguments, 0);
                commandResult.Response = player.BuyItemFromName(name);
                return;

            }
            case 2:
            {
                
                if (!player.IsAlive)
                {
                    commandResult.Response = player.GetAvailableCategories(true);
                    return;
                }
                if (int.TryParse(arguments.Array[1], out var category) &&
                    int.TryParse(arguments.Array[2], out var item))
                {
                    commandResult.Response = player.BuyItemFromId(category, item);
                    return;
                }
                var name1 = FormatArguments(arguments, 0);
                commandResult.Response = player.BuyItemFromName(name1);
                return;
                
            }
            case >2:                 
                var name2 = FormatArguments(arguments, 0);
                commandResult.Response = player.BuyItemFromName(name2);
                return;
            default:
                commandResult.Response = player.GetAvailableCategories();
                return;
        }
        
    }
    public static string FormatArguments(ArraySegment<string> sentence, int index)
    {
        var sb = StringBuilderPool.Shared.Rent();
        foreach (var word in sentence.Segment(index))
        {
            sb.Append(word);
            sb.Append(" ");
        }
        var msg = sb.ToString();
        StringBuilderPool.Shared.Return(sb);
        return msg;
    }
}