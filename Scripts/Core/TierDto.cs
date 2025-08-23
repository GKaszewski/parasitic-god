using System.Collections.Generic;

namespace ParasiticGod.Scripts.Core;

public class TierDto
{
    public Follower.FollowerTier TierEnum { get; set; }
    public long Threshold { get; set; }
    public string ScenePath { get; set; }
}

public class TierListDto
{
    public List<TierDto> Tiers { get; set; }
}