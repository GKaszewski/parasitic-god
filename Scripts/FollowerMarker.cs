using Godot;

namespace ParasiticGod.Scripts;

[GlobalClass]
public partial class FollowerMarker : Marker2D
{
    public bool IsOccupied { get; private set; }
    public Follower FollowerInstance { get; private set; }

    public void PlaceFollower(Follower followerInstance)
    {
        if (IsOccupied) return;
        AddChild(followerInstance);
        followerInstance.Position = Vector2.Zero;
        IsOccupied = true;
        FollowerInstance = followerInstance;
    }

    public void RemoveFollower()
    {
        if (!IsOccupied) return;
        FollowerInstance.QueueFree();
        FollowerInstance = null;
        IsOccupied = false;
    }
}