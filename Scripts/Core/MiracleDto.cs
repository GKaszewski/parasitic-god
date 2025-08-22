using System.Collections.Generic;
using ParasiticGod.Scripts.Core.Effects;

namespace ParasiticGod.Scripts.Core;

public class EffectDto
{
    public string Type { get; set; }
    // --- For "AddResource" Effect ---
    public ResourceType TargetResource { get; set; }
    public double Value { get; set; }

    // --- For "ApplyBuff" Effect ---
    public ApplyBuffEffect.BuffTarget TargetBuffStat { get; set; }
    public float Multiplier { get; set; }
    public double Duration { get; set; }

    // --- For "ConvertResource" Effect ---
    public ResourceType FromResource { get; set; }
    public double FromAmount { get; set; }
    public ResourceType ToResource { get; set; }
    public double ToAmount { get; set; }
    
    // --- For "ModifyStat" Effect ---
    public ModifyStatEffect.Stat TargetStat { get; set; }
    public ModifyStatEffect.Operation Op { get; set; }
    
    public List<string> MiraclesToUnlock { get; set; }
}

public class MiracleDto
{
    public string Name { get; set; }
    public double FaithCost { get; set; }
    public long FollowersRequired { get; set; }
    public bool UnlockedByDefault { get; set; }
    public List<EffectDto> Effects { get; set; }
}