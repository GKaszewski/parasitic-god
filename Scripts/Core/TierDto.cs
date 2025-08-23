using System.Collections.Generic;
using System.Numerics;

namespace ParasiticGod.Scripts.Core;

public class TierDto
{
    public Follower.FollowerTier TierEnum { get; set; }
    public long Threshold { get; set; }
    public string ImagePath { get; set; }
    public Vector2 Scale { get; set; } = Vector2.One;
}

public class TierListDto
{
    public List<TierDto> Tiers { get; set; }
}