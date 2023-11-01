﻿using System;

namespace DayLight.DiscordSync.Dependencys;

public struct Warn
{
    public int Id { get; set; }
    public string Reason { get; set; }
    public float Points { get; set; }
    public string WarnerUsername { get; set; }
    public DateTime Date { get; set; }
}

