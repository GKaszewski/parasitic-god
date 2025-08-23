using Godot;

namespace ParasiticGod.Scripts.Components;

[GlobalClass]
public partial class ModdableVisual : Node2D
{
    [Export] private Sprite2D _sprite;
    public Follower.FollowerTier Tier { get; private set; }

    public override void _ExitTree()
    {
        if (_sprite != null)
        {
            _sprite.Texture = null;
        }
    }

    public void Initialize(Follower.FollowerTier tier, Texture2D texture, Vector2 scale)
    {
        Tier = tier;
        if (_sprite != null && texture != null)
        {
            _sprite.Texture = texture;
            _sprite.Scale = scale;
        }
    }
}