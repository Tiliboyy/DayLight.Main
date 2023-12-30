using System.Collections.Generic;

namespace DayLight.Dependencys.Models;

public class Achievements
{

    public static List<Achivement> AllAchivements = new()
    {
        //done
        new Achivement
        {
            Name = "Diabetes",
            Description = "Esse 8 Candies in einer Runde",
            Id = 1,
            Reward = 5000
        },
        //done
        new Achivement
        {
            Name = "Yo was geht?",
            Description = "Töte 5 Leute mit einem Pink Candy.)",
            Id = 2,
            Reward = 10000
        },
        //done
        new Achivement
        {
            Name = "Doch nicht so useless, huh?",
            Description = "Überlebe 20 Minuten als Guard.",
            Id = 3,
            Reward = 10000
        },
        //done
        new Achivement
        {
            Name = "Man bist du schlecht",
            Description = "Stirb in den ersten 30 Sekunden.",
            Id = 4,
            Reward = 1000
        },
        //done
        new Achivement
        {
            Name = "Double Digits",
            Description = "Habe 10 oder mehr Zuschauer.",
            Id = 5,
            Reward = 10000
        },
        //done
        new Achivement
        {
            Name = "Thats how you do it Bois",
            Description = "Töte als Guard ein SCP.",
            Id = 6,
            Reward = 5000
        },
        //done
        new Achivement
        {
            Name = "Skill Issue",
            Description = "Sterbe mindestens 5 mal in einer Runde.",
            Id = 8,
            Reward = 10000
        },
        //done
        new Achivement
        {
            Name = "Meisterdetektiv",
            Description = "Finde heraus was passiert, wenn man SCP-207 und SCP-1853 zusammen nimmt.",
            Id = 9,
            Reward = 1000
        },
        //done
        new Achivement
        {
            Name = "EAlight Gaming",
            Description = "Gebe im Gamestore 50k DayLight Bits in einer Runde aus.",
            Id = 10,
            Reward = 20000
        },
        //done
        new Achivement
        {
            Name = "Guter Gaming Stuhl",
            Description = "Töte als Mensch mehr als 12 Leute ohne zu sterben.",
            Id = 11,
            Reward = 15000
        },
        //done
        new Achivement
        {
            Name = "Doom Guy",
            Description = "Überlebe mindestens 2 NTF Waves als Chaos",
            Reward = 5000,
            Id = 12
        },
        //done
        new Achivement
        {
            Name = "Twitter Moment",
            Description = "Deaktiviere die Nuke 3 mal in einer Runde",
            Reward = 15000,
            Id = 13
        },
        //done
        new Achivement
        {
            Name = "Be my daddy please",
            Description = "Lass dich als MTF von einem D-Boy cuffen.",
            Reward = 10000,
            Id = 14
        },
        //done
        new Achivement
        {
            Name = "Survivor",
            Description = "Überlebe eine ganze Runde als Guard, ClassD oder Scientist ohne zu sterben.",
            Reward = 15000,
            Id = 15
        },
        //done
        new Achivement
        {
            Name = "Last Stand",
            Description = "Sei der letzte menschlicher Spieler in der Runde während mehr als 10 Spieler Online sind und ein SCP lebt.",
            Reward = 10000,
            Id = 16
        },
        //done
        new Achivement
        {
            Name = "Psychopath",
            Description = "Töte als Scientist mindestens 5 D-Boys in einer Runde.",
            Reward = 5000,
            Id = 17
        },
        //done
        new Achivement
        {
            Name = "Worth it",
            Description = "Töte einen Scientist als ClassD mit einer Micro HID.",
            Reward = 5000,
            Id = 18
        },
        //done
        new Achivement
        {
            Name = "Pew Pew Pew",
            Description = "Bekomme die COM-45 in SCP-914",
            Reward = 5000,
            Id = 19
        },
        //done
        new Achivement
        {
            Name = "Drug addict",
            Description = "Schlucke 20 Pillen in einer Runde ohne zu sterben",
            Reward = 5000,
            Id = 20
        },
        //done
        new Achivement
        {
            Name = "The Collector",
            Description = "Sammle mehr als 20 verschiedene Gegenstände ohne zu sterben.",
            Reward = 15000,
            Id = 21
        },
        //done
        new Achivement
        {
            Name = "Overcharge",
            Description = "Erreiche als SCP-079 Level 5 in unter 10 minuten.",
            Reward = 10000,
            Id = 22
        },
        //done
        new Achivement
        {
            Name = "Ultrakill",
            Description = "Töte 7 Personen in unter 15 Sekunden.",
            Reward = 25000,
            Id = 23
        },
        //done
        new Achivement
        {
            Name = "Für die Horde!",
            Description = "Habe als SCP-049 mehr als 10 Zombies am leben.",
            Reward = 10000,
            Id = 24
        },
        //done
        new Achivement
        {
            Name = "Yippie!",
            Description = "Trinke 4 Colas!.",
            Reward = 5000,
            Id = 25
        },
        //done
        new Achivement
        {
            Name = "Colabieren",
            Description = "Habe einen negative und positive Cola gleichzeitig.",
            Reward = 5000,
            Id = 26
        },
        //done
        //done
        new Achivement
        {
            Name = "Funny",
            Description = "Treffe drei SCPs mit einem Pink Candy.",
            Reward = 5000,
            Id = 30
        },
        new Achivement
        {
            Name = "Kaufrausch",
            Description = "Kaufe 10 Items im Gamestore ohne zu sterben.",
            Reward = 10000,
            Id = 27
        },
        //done
        new Achivement
        {
            Name = "Brick",
            Description = "Spawne als Brick",
            Reward = 5000,
            Id = 28
        },
        //done
        new Achivement
        {
            Name = "Welp..",
            Description = "Habe den Doctor und SCP-106 effekt gleichzeitig",
            Reward = 15000,
            Id = 29
        },
        //done
        new Achivement
        {
            Name = "Ich bin nützlich!",
            Description = "Töte 5 Personen als Zombie",
            Reward = 5000,
            Id = 31
        },
        //done
        new Achivement
        {
            Name = "GCP",
            Description = "Treffe drei SCPs mit einer Granate",
            Reward = 10000,
            Id = 32
        },
        //done
        new Achivement
        {
            Name = "Oh no",
            Description = "Sterbe an der Decontamination",
            Reward = 1000,
            Id = 33
        },
        //done
        new Achivement
        {
            Name = "Laser Tag",
            Description = "Töte ein SCP mit einem Partice Disruptor",
            Reward = 5000,
            Id = 34
        },
        //done
        new Achivement
        {
            Name = ":Troll:",
            Description = "Töte ein Teammitglied",
            Reward = 1000,
            Id = 35
        },
        //done
        new Achivement
        {
            Name = "Missinput",
            Description = "Sterbe in einer Death Zone",
            Reward = 1000,
            Id = 36
        },
        //done
        new Achivement
        {
            Name = "Speedrun",
            Description = "Entkomme in unter 2 Minuten",
            Reward = 10000,
            Id = 37
        },
    };

    public struct Achivement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Reward { get; set; }
    }
}
