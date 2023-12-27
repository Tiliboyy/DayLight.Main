using DayLight.Core;
using DayLight.Core.API;
using DayLight.Core.API.Database;
using DayLight.Core.API.Features;
using DayLight.Dependencys.Stats;
using PlayerRoles;
using System;
using System.Linq;
using Player = Exiled.API.Features.Player;
using Round = Exiled.API.Features.Round;

namespace DayLight.GameStore;

public static class Extensions
{
    
    public static bool SetMoney(this Player player, float money)
    {
        if (player == null) return false;
        if (player.DoNotTrack) return false;
        var playerID = ulong.Parse(player.RawUserId.Split('@')[0]);
        var players = DayLightDatabase.Database.GetCollection<DatabasePlayer>("players");
        var dbplayer = players.FindOne(x => x.SteamID != null && x.SteamID == playerID);

        if (dbplayer == null) return false;
        dbplayer.Stats.Money = money;
        if (dbplayer.Stats.Money < 0) dbplayer.Stats.Money = 0;
        players.Update(dbplayer);
        return true;
    }
    public static string GetAvailableCategories(this Player player, bool ShowAll = false)
    {
        var list = GameStorePlugin.Instance.Config.Categorys.OrderBy(category => category.id).ToList();
        var categories = list;
        if (GameStorePlugin.Instance.Config.ShowOnlyAvalibleItems && !ShowAll)
            categories = list.Where(category => category.AllowedRoles.Contains(player.Role.Type) || category.AllowedRoles.Contains(RoleTypeId.None) && !player.IsScp).ToList();
        var category = $"";;
        var i = 1;
        foreach (var categoryitem in categories)
        {
            category += $"\n[{i}] " + categoryitem.Name + "\n        " + categoryitem.Description;
            i++;
        }
        if (category == "")
            category = GameStorePlugin.Instance.Translation.NothingToBuy;
        return category + "";
    }
    public static string GetAvailableItems(this Player player,int category)
    {
        var items = $"";
        var e = GameStorePlugin.Instance.Config.Categorys.OrderBy(category1 => category1.id).ToList();
        var categories = e;
        if (GameStorePlugin.Instance.Config.ShowOnlyAvalibleItems)
            categories = e.Where(VARIABLE => VARIABLE.AllowedRoles.Contains(player.Role.Type) || VARIABLE.AllowedRoles.Contains(RoleTypeId.None) && !player.IsScp).ToList();
        if (category > categories.Count)
            return GameStorePlugin.Instance.Translation.ItemDoesNotExist;
        var i = 0;
        foreach (var categorynum in categories)
        {
            i++;
            if(i != category) continue;
            items = categorynum.Items.Aggregate(items, (current, item) => current + $"\n[{item.Id}] {item.Name} - {item.Price} {GameStorePlugin.Instance.Translation.CurrencyName}");
        }
        
        return items + "";
    }
    public static string BuyItemFromName(this Player player, string Name)
    {
        var advancedPlayer = AdvancedPlayer.Get(player);
        if (!Round.IsStarted)
        {
            return GameStorePlugin.Instance.Translation.RoundNotStarted;
        }
        var list = GameStorePlugin.Instance.Config.Categorys.OrderBy(category1 => category1.id).ToList();
        var foundwithrole = false;
        var notexisting = false;
        var nomoney = false;
        var notfullinventory = false;
        foreach (var category1 in list)
        {
            var itemlist = category1.Items.OrderBy(price => price.Id);
            var itembytype = Enum.TryParse(Name, out ItemType itemType);
            foreach (var items in itemlist)
            {
                if (itembytype)
                {
                    if (!items.ItemTypes.Contains(itemType)) continue;
                }
                else
                {
                    if(!string.Equals(items.Name.Replace(" ", ""), Name.Replace(" ", ""), StringComparison.CurrentCultureIgnoreCase)) continue;
                }
                notexisting = true;
                var num = $"{category1.id}{items.Id}";
                if (!int.TryParse(num, out var result))
                {
                    continue;
                }
                if (!category1.AllowedRoles.Contains(player.Role.Type) && !category1.AllowedRoles.Contains(RoleTypeId.None) || player.IsScp)
                {
                    continue;
                }
                foundwithrole = true;
                if (player.Items.Count >= 8 && items.IgnoreFullInventory == false)
                {
                    continue;
                }

                notfullinventory = true;


                if (!DayLightDatabase.GameStore.CanRemoveMoneyFromPlayer(player, items.Price))
                {
                    continue;
                }

                nomoney = true;

                if (advancedPlayer != null && advancedPlayer.GameStoreBoughtItems.ContainsKey(result))
                {
                    if (advancedPlayer.GameStoreBoughtItems[result] >=
                        items.MaxAmount)
                    {
                        continue;
                    }

                    advancedPlayer.GameStoreBoughtItems[result]++;
                }
                else
                {
                    advancedPlayer.GameStoreBoughtItems.Add(result, 1);
                }

                DayLightDatabase.GameStore.BuyItem(player, items);
                return GameStorePlugin.Instance.Translation.BoughtItem.Replace("(itemname)", items.Name)
                    .Replace("(itemprice)", items.Price.ToString());
            }
        }

        if (!notexisting)
        {
            return GameStorePlugin.Instance.Translation.ItemDoesNotExist;
        }
        if (!foundwithrole)
        {
            return GameStorePlugin.Instance.Translation.WrongeRole;
        }
        if (!notfullinventory)
        {
            return GameStorePlugin.Instance.Translation.FullInventory;
        }

        return !nomoney ? GameStorePlugin.Instance.Translation.CantAfford : GameStorePlugin.Instance.Translation.MaxAmountReached;

    }
    public static string BuyItemFromId(this Player player, int category, int item, bool ShowAll = false)
    {
        var advancedPlayer = AdvancedPlayer.Get(player);
        if (!Round.IsStarted)
        {
            return GameStorePlugin.Instance.Translation.RoundNotStarted;
        }

        if (player == null)
        {
            return GameStorePlugin.Instance.Translation.ErrorMessage.Replace("(error)", "Player is Null");
        }

        var Categorys = GameStorePlugin.Instance.Config.Categorys.OrderBy(category1 => category1.id).ToList();
        if (GameStorePlugin.Instance.Config.ShowOnlyAvalibleItems)
        {
            Categorys = Categorys.Where(x => x.AllowedRoles.Contains(player.Role.Type) || x.AllowedRoles.Contains(RoleTypeId.None)).ToList();
        }
        if (category > Categorys.Count)
        {
            return GameStorePlugin.Instance.Translation.CategoryDoesNotExist;
        }

        var list = Categorys;
        var i = 0;
        foreach (var category1 in list)
        {
            i++;
            if (i != category) continue;
            var itemlist = category1.Items.OrderBy(price => price.Id).ToList();
            foreach (var items in itemlist.Where(items => item == items.Id))
            {
                var num = $"{category1.id}{items.Id}";
                if (!int.TryParse(num, out var result))
                {
                    Logger.Warn($"\nCategory ID: {category1.id} \nItem ID:{items.Id} is invalid");
                    return GameStorePlugin.Instance.Translation.ErrorMessage.Replace("(error)", $"Category Id: {category1.id} or {items.Id} is Invalid");
                }

                if (category1.AllowedRoles == null)
                {
                    Logger.Error("Allowed Roles is null!");
                    return GameStorePlugin.Instance.Translation.ErrorMessage.Replace("(error)", $"Allowed roles from {category1.Name} is null");
                }
                if (!category1.AllowedRoles.Contains(player.Role.Type) && !category1.AllowedRoles.Contains(RoleTypeId.None) || player.IsScp)
                {
                    return GameStorePlugin.Instance.Translation.WrongeRole;
                }
                if (player.IsInventoryFull && items.IgnoreFullInventory == false)
                {
                    return GameStorePlugin.Instance.Translation.FullInventory;
                }

                if (!DayLightDatabase.GameStore.CanRemoveMoneyFromPlayer(player, items.Price))
                {
                    return GameStorePlugin.Instance.Translation.CantAfford;
                }

                if (advancedPlayer != null && advancedPlayer.GameStoreBoughtItems.ContainsKey(result))
                {

                    if (advancedPlayer.GameStoreBoughtItems[result] >=
                        items.MaxAmount)
                    {
                        return GameStorePlugin.Instance.Translation.MaxAmountReached;
                    }
                    advancedPlayer.GameStoreBoughtItems[result]++;
                }
                else
                {
                    if (advancedPlayer != null) advancedPlayer.GameStoreBoughtItems.Add(result, 1);
                }
                DayLightDatabase.GameStore.BuyItem(player, items);
                return GameStorePlugin.Instance.Translation.BoughtItem.Replace("(itemname)", items.Name)
                    .Replace("(itemprice)", items.Price.ToString());
            }
            break;
        }

        return GameStorePlugin.Instance.Translation.ItemDoesNotExist;


    }
}