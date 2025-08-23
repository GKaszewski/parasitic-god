using Godot;

namespace ParasiticGod.Scripts.Core;

[GlobalClass]
public partial class TierDefinition : Resource
{
    [Export] public PackedScene Scene { get; set; }
    [Export] public long Threshold { get; set; }
    [Export] public Follower.FollowerTier TierEnum { get; set; }
}