using CommandSystem;
using DayLight.Core.API.CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using PlayerRoles;
using System;
using System.Diagnostics.CodeAnalysis;
using Vector3 = UnityEngine.Vector3;


namespace DayLight.Misc.Commands;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
internal class Size : CustomCommand
{
    
    public override string Command { get; } = "size";

    public override string[] Aliases { get; } = Array.Empty<string>();

    public override string Description { get; } = "Sets the size of all users or a user";
    protected override string Permission { get; } = "AT.size";

    protected override bool Respond(ArraySegment<string> arguments, Player player, out string response)
    {


        if (arguments.Count < 1)
        {
            response = "Usage:\nsize (player id / name) or (all / *)) (x value) (y value) (z value)" +
                       "\nsize reset";
            return false;
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

                response = $"Everyone's size has been reset";
                return true;
            case "*":
            case "all":
                if (arguments.Count != 4)
                {
                    response = "Usage: size (all / *) (x) (y) (z)";
                    return false;
                }

                if (!float.TryParse(arguments.At(1), out float xval))
                {
                    response = $"Invalid value for x size: {arguments.At(1)}";
                    return false;
                }

                if (!float.TryParse(arguments.At(2), out float yval))
                {
                    response = $"Invalid value for y size: {arguments.At(2)}";
                    return false;
                }

                if (!float.TryParse(arguments.At(3), out float zval))
                {
                    response = $"Invalid value for z size: {arguments.At(3)}";
                    return false;
                }

                foreach (Player ply in Player.List)
                {
                    if (ply.Role == RoleTypeId.Spectator || ply.Role == RoleTypeId.None)
                        continue;

                    SetPlayerScale(ply, xval, yval, zval);
                }

                response = $"Everyone's scale has been set to {xval} {yval} {zval}";
                return true;
            default:
                if (arguments.Count != 4)
                {
                    response = "Usage: size (player id / name) (x) (y) (z)";
                    return false;
                }

                Player pl = Player.Get(arguments.At(0));
                if (pl == null)
                {
                    response = $"Player not found: {arguments.At(0)}";
                    return false;
                }

                if (!float.TryParse(arguments.At(1), out float x))
                {
                    response = $"Invalid value for x size: {arguments.At(1)}";
                    return false;
                }

                if (!float.TryParse(arguments.At(2), out float y))
                {
                    response = $"Invalid value for y size: {arguments.At(2)}";
                    return false;
                }

                if (!float.TryParse(arguments.At(3), out float z))
                {
                    response = $"Invalid value for z size: {arguments.At(3)}";
                    return false;
                }

                SetPlayerScale(pl, x, y, z);
                response = $"Player {pl.Nickname}'s scale has been set to {x} {y} {z}";
                return true;
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