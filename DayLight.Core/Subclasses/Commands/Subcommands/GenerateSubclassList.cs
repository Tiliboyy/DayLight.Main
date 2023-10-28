using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomRole = Exiled.CustomRoles.API.Features.CustomRole;


namespace DayLight.Core.Subclasses.Commands.Subcommands;

internal class GenerateSubclassList : ICommand
{
    public string Command { get; } = "list";

    public string[] Aliases { get; } = Array.Empty<string>();

    public string Description { get; } = "Generates a Subclass List in the Subclass File dir";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        
        if (!sender.CheckPermission("subclasses.list"))
        {
            response = $"You do not have the correct permission to use this command (subclasses.list)";

            return false;
        }

        string Text = "Rollen:\n";
        var ClassD = new List<CustomRole>();
        var Scientists = new List<CustomRole>();

        var MTF = new List<CustomRole>();

        var Chaos = new List<CustomRole>();
        var Scps = new List<CustomRole>();

        foreach (var role in Manager.Subclasses.Select(type => CustomRole.Get(type.ClassName))
                     .Where(role => role != null))
        {
            switch (RoleExtensions.GetTeam(role.Role))
            {
                case Team.SCPs:
                    Scps.Add(role);
                    break;
                case Team.FoundationForces:
                    MTF.Add(role);
                    break;
                case Team.ChaosInsurgency:
                    Chaos.Add(role);
                    break;
                case Team.Scientists:
                    Scientists.Add(role);
                    break;
                case Team.ClassD:
                    ClassD.Add(role);
                    break;
                case Team.Dead:
                    break;
                case Team.OtherAlive:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        Text += "\n**ClassD**\n";

        Text = ClassD.OrderBy(X => X.Role).Aggregate(Text,
            (current, customRole) =>
                current +
                $" - {customRole.Name} ({customRole.Role})\n        {customRole.Description.Replace("\n", " ")}\n\n");
        Text += "\n**Scientists**\n";

        Text = Scientists.OrderBy(X => X.Role).Aggregate(Text,
            (current, customRole) =>
                current +
                $" - {customRole.Name} ({customRole.Role})\n        {customRole.Description.Replace("\n", " ")}\n\n");
        Text += "\n**MTF**\n";

        Text = MTF.OrderBy(X => X.Role).Aggregate(Text,
            (current, customRole) =>
                current +
                $" - {customRole.Name} ({customRole.Role})\n        {customRole.Description.Replace("\n", " ")}\n\n");
        Text += "\n**Chaos Insurgency**\n";

        Text = Chaos.OrderBy(X => X.Role).Aggregate(Text,
            (current, customRole) =>
                current +
                $" - {customRole.Name} ({customRole.Role})\n        {customRole.Description.Replace("\n", " ")}\n\n");
        Text += "\n**SCP**\n";

        Text = Scps.OrderBy(X => X.Role).Aggregate(Text,
            (current, customRole) =>
                current +
                $" - {customRole.Name} ({customRole.Role})\n        {customRole.Description.Replace("\n", " ")}\n\n");
        var filePath = Path.Combine(Paths.Configs, "SubclassList.txt");

        File.WriteAllText(filePath, Text);

        response = $"{Text} \n\nSuccess written text to: " + filePath;
        return true;
    }
}