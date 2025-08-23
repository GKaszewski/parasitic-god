using Godot;

namespace ParasiticGod.Scripts.Core;

[GlobalClass]
public partial class TierDefinition : Resource
{
    [Export] public Texture2D Texture { get; set; }
    [Export] public long Threshold { get; set; }
    [Export] public Follower.FollowerTier TierEnum { get; set; }
    [Export] public Vector2 Scale { get; set; } = Vector2.One;
}