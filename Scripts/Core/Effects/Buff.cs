using System;

namespace ParasiticGod.Scripts.Core.Effects;

public class Buff
{
    public Guid InstanceId { get; } = Guid.NewGuid(); // Unique identifier
    public string BuffId { get; set; } // Identifier for the type of buff
    public string Name { get; set; } // For display purposes
    public float Multiplier { get; set; } = 1.0f;
    public double Duration { get; set; }
}