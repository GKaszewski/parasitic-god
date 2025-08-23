using System.Collections.Generic;
using Godot;
using Godot.Collections;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scripts;

[GlobalClass]
public partial class PopulationVisualizer : Node
{
    [Export] private Node2D _markersContainer;
    [Export] private int _unitsPerMarker = 5;
    [Export] private Array<TierDefinition> _tiers;
    
    private readonly List<FollowerMarker> _markers = [];
    private long _lastKnownUnitCount = -1;
    private int _lastKnownTierIndex = -1;
    private bool _isUpdating = false;
    
    public override void _Ready()
    {
        foreach (var child in _markersContainer.GetChildren())
        {
            if (child is FollowerMarker marker)
            {
                _markers.Add(marker);
            }
        }

        GameBus.Instance.StateChanged += OnStateChanged;
    }
    
    public override void _ExitTree()
    {
        GameBus.Instance.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState newState)
    {
        if (_isUpdating) return;
        
        var currentUnitCount = (long)newState.Get(Stat.Followers);

        var currentMarkersToShow = (int)currentUnitCount / _unitsPerMarker;
        var lastMarkersToShow = (int)_lastKnownUnitCount / _unitsPerMarker;
        var newTierIndex = GetTierIndex(currentUnitCount);

        if (currentMarkersToShow != lastMarkersToShow || newTierIndex != _lastKnownTierIndex)
        {
            UpdateVisualsProgressively(currentUnitCount, newTierIndex);
        }
    }
    
    private int GetTierIndex(long currentUnitCount)
    {
        for (var i = _tiers.Count - 1; i >= 0; i--)
        {
            if (currentUnitCount >= _tiers[i].Threshold)
            {
                return i;
            }
        }
        return -1;
    }

    private async void UpdateVisualsProgressively(long currentUnitCount, int newTierIndex)
    {
        _isUpdating = true;

        if (newTierIndex < 0)
        {
            _isUpdating = false;
            return;
        }

        var followersToShow = (int)currentUnitCount / _unitsPerMarker;
        var currentTier = _tiers[newTierIndex];

        for (var i = 0; i < _markers.Count; i++)
        {
            var marker = _markers[i];
            var needsChange = false;

            if (i < followersToShow)
            {
                if (!marker.IsOccupied || _lastKnownTierIndex != newTierIndex)
                {
                    if (marker.IsOccupied) marker.RemoveFollower();
                    var followerInstance = currentTier.Scene.Instantiate<Follower>();
                    marker.PlaceFollower(followerInstance);
                    needsChange = true;
                }
            }
            else
            {
                if (marker.IsOccupied)
                {
                    marker.RemoveFollower();
                    needsChange = true;
                }
            }

            if (needsChange)
            {
                await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
            }
        }

        _lastKnownUnitCount = currentUnitCount;
        _lastKnownTierIndex = newTierIndex;
        _isUpdating = false;
    }
}