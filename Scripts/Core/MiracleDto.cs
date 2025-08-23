using System.Collections.Generic;
using ParasiticGod.Scripts.Core.Effects;

namespace ParasiticGod.Scripts.Core;

public class EffectDto
{
    public string Type { get; set; }

    // --- For "AddResource" Effect ---
    public Stat TargetResource { get; set; }
    public double Value { get; set; }

    // --- For "ApplyBuff" Effect ---
    public float Multiplier { get; set; }
    public double Duration { get; set; }

    // --- For "ConvertResource" Effect ---
    public Stat FromResource { get; set; }
    public double FromAmount { get; set; }
    public Stat ToResource { get; set; }
    public double ToAmount { get; set; }

    // --- For "ModifyStat" Effect ---
    public Stat TargetStat { get; set; }
    public ModifyStatEffect.Operation Op { get; set; }

    public List<string> MiraclesToUnlock { get; set; }
}

public class MiracleDto
{
    public string Name { get; set; }
    public double FaithCost { get; set; }
    public long FollowersRequired { get; set; }
    public double ProductionRequired { get; set; }
    public bool UnlockedByDefault { get; set; }
    public string AdvancesToAge { get; set; }
    public List<EffectDto> Effects { get; set; }
}