using CommandSystem;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using PlayerRoles;
using System;
using Vector3 = UnityEngine.Vector3;


namespace DayLight.Misc.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal class Size : CustomCommand
{
    
    public override string Command { get; } = "size";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Sets the size of all users or a user";
    public override string Permission { get; } = "AT.size";

    protected override void Respond(ArraySegment<string> arguments, Player player, ref CommandResult commandResult)
    {


        if (arguments.Count < 1)
        {
            commandResult.Response = "Usage:\nsize (player id / name) or (all / *)) (x value) (y value) (z value)" +
                       "\nsize reset";
            commandResult.Success = false;
            return;
        }

        switch (arguments.At(0))
        {
            case "reset":
                foreach (Player ply in Player.List)
                {
                    if (ply.Role == RoleTypeId.Spectator || ply.Role == RoleTypeId.None)
                        continue;

                    SetPlayerScale(ply, 1, 1, 1);
                }

                commandResult.Response = $"Everyone's size has been reset";
                commandResult.Success = true;
                return;
            case "*":
            case "all":
                if (arguments.Count != 4)
                {
                    commandResult.Response = "Usage: size (all / *) (x) (y) (z)";
                    commandResult.Success = false;
                    return;
                }

                if (!float.TryParse(arguments.At(1), out float xval))
                {
                    commandResult.Response = $"Invalid value for x size: {arguments.At(1)}";
                    commandResult.Success = false;
                    return;
                    
                }

                if (!float.TryParse(arguments.At(2), out float yval))
                {
                    commandResult.Response = $"Invalid value for y size: {arguments.At(2)}";
                    commandResult.Success = false;
                    return;
                    
                }

                if (!float.TryParse(arguments.At(3), out float zval))
                {
                    commandResult.Response = $"Invalid value for z size: {arguments.At(3)}";
                    commandResult.Success = false;
                    return;
                    
                }

                foreach (Player ply in Player.List)
                {
                    if (ply.Role == RoleTypeId.Spectator || ply.Role == RoleTypeId.None)
                        continue;

                    SetPlayerScale(ply, xval, yval, zval);
                }

                commandResult.Response = $"Everyone's scale has been set to {xval} {yval} {zval}";
                commandResult.Success = true;
                return;
            default:
                if (arguments.Count != 4)
                {
                    commandResult.Response = "Usage: size (player id / name) (x) (y) (z)";
                    commandResult.Success = false;
                    return;
                }

                Player pl = Player.Get(arguments.At(0));
                if (pl == null)
                {
                    commandResult.Response = $"Player not found: {arguments.At(0)}";
                    commandResult.Success = false;
                    return;
                }

                if (!float.TryParse(arguments.At(1), out float x))
                {
                    commandResult.Response = $"Invalid value for x size: {arguments.At(1)}";
                    commandResult.Success = false;
                    return;
                    
                }

                if (!float.TryParse(arguments.At(2), out float y))
                {
                    commandResult.Response = $"Invalid value for y size: {arguments.At(2)}";
                    commandResult.Success = false;
                    return;
                }

                if (!float.TryParse(arguments.At(3), out float z))
                {
                    commandResult.Response = $"Invalid value for z size: {arguments.At(3)}";
                    commandResult.Success = false;
                    return;
                }

                SetPlayerScale(pl, x, y, z);
                commandResult.Response = $"Player {pl.Nickname}'s scale has been set to {x} {y} {z}";
                return;
        }

    }
    public static void SetPlayerScale(Player target, float x, float y, float z)
    {
        try
        {
            target.Scale = new Vector3(x, y, z);
        }
        catch (Exception e)
        {
            Log.Info($"Set Scale error: {e}");
        }
    }

    
}