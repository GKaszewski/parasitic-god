using System;

namespace ParasiticGod.Scripts.Core.Effects;

public class Buff
{
    public Guid Id { get; } = Guid.NewGuid(); // Unique identifier
    public string Name { get; set; } // For display purposes
    public float Multiplier { get; set; } = 1.0f;
    public double Duration { get; set; }
}