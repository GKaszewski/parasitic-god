using System;

namespace ParasiticGod.Scripts.Core.Effects;

public class Buff
{
    public Guid InstanceId { get; } = Guid.NewGuid();
    public string BuffId { get; set; }
    public string Name { get; set; }
    public Stat TargetStat { get; set; }
    public float Multiplier { get; set; } = 1.0f;
    public double Duration { get; set; }
}