using Godot;

namespace ParasiticGod.Scripts;

[GlobalClass]
public partial class Follower : Node2D
{
    public enum FollowerTier { Tier1, Tier2, Tier3, Tier4, Tier5, Tier6, Tier7, Tier8, Tier9, Tier10 }
    [Export] public FollowerTier Tier { get; private set; }
}