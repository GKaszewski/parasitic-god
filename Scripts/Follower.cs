using Godot;

namespace ParasiticGod.Scripts;

[GlobalClass]
public partial class Follower : Node2D
{
    public enum FollowerTier { Tier1, Tier2, Tier3, Tier4, Tier5 }
    [Export] public FollowerTier Tier { get; private set; }
}