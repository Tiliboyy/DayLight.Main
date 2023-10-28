﻿using Exiled.API.Interfaces;
using Neuron.Core.Meta;
using Neuron.Modules.Configs.Localization;
using System;

namespace DayLight.GameStore.Configs;

[Automatic]
public class GameStoreTranslation : Translations<GameStoreTranslation>
{
    public string CurrencyName { get; set; } = "Daylight Bits";
    public string NothingToBuy { get; set; } = "Du kannst mit deiner Rolle nichts kaufen.";
    public string SetMoneyHintText { get; set; } = "Deine Daylight Bits wurden auf (money) gesetzt.";
    
    public string PayMoneyHintText { get; set; } = "Du hast (money) DayLight Bits von (sender) erhalten.";
    public string BoughtItemBHint { get; set; } = "Du hast (item) gekauft.";
    public string BoughtItem { get; set; } = "Du hast (itemname) für (itemprice) Daylight Bits gekauft";
    public string ErrorMessage { get; set; } = "Error please contact server staff with this: (error)";

    public string MaxAmountReached { get; set; } = "Du hast schon genug davon gekauft.";

    public string CantAfford { get; set; } = "Du kannst dir dieses Item nicht leisten.";
    public string WrongeRole { get; set; } = "Deine Rolle erlaubt es dir nicht dieses Item zu kaufen.";

    public string FullInventory { get; set; } = "Dein Inventar ist voll";

    public string DntMessage { get; set; } = "Du hast Do not Track aktiviert. Deakiviere DNT um den GameStore verwenden zu können";

    public string DisabledStore { get; set; } = "Der GameStore ist momentan deaktiviert";

    public string RoundNotStarted { get; set; } = "Die Runde hat noch nicht gestartet";

    public string CategoryDoesNotExist { get; set; } = "Diese Kategorie existiert nicht!";

    public string BalanceMessage { get; set; } = "Du hast (balance) Daylight Bits";

    public string ItemDoesNotExist { get; set; } = "Dieses Item exisitert nicht!";
}