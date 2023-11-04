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

    protected override bool Respond(ArraySegment<string> arguments, Player player, out string response)
    {

        if (player.DoNotTrack)
        {
            response = GameStorePlugin.Instance.Translation.DntMessage;
            return true;
        }
        if (!GameStorePlugin.EnableGamestore)
        {
            response = GameStorePlugin.Instance.Translation.DisabledStore;
            return true;
        }


        if (arguments.Array == null)
        {
            response = "What the fuck did you do";
            return false;
        }

        switch (arguments.Count)
        {
            case 0:
                
                if (!player.IsAlive)
                {
                    response = player.GetAvailableCategories(true);
                    return true;
                }
                response = player.GetAvailableCategories();
                return true;
            case 1:
            {
                

                if (int.TryParse(arguments.Array[1], out var argument1))
                {
                    if (!player.IsAlive)
                    {
                        response = player.GetAvailableItems(argument1);
                        return true;
                    }
                    response = player.GetAvailableItems(argument1);
                    return true;
                }
                if (!player.IsAlive)
                {
                    response = GameStorePlugin.Instance.Translation.WrongeRole;
                    return true;
                }
                var name = FormatArguments(arguments, 0);
                response = player.BuyItemFromName(name);
                return true;

            }
            case 2:
            {
                
                if (!player.IsAlive)
                {
                    response = player.GetAvailableCategories(true);
                    return true;
                }
                if (int.TryParse(arguments.Array[1], out var category) &&
                    int.TryParse(arguments.Array[2], out var item))
                {
                    response = player.BuyItemFromId(category, item);
                    return true;
                }
                var name1 = FormatArguments(arguments, 0);
                response = player.BuyItemFromName(name1);
                return true;
                
            }
            case >2:                 
                var name2 = FormatArguments(arguments, 0);
                response = player.BuyItemFromName(name2);
                return true;
            default:
                response = player.GetAvailableCategories();
                return true;
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