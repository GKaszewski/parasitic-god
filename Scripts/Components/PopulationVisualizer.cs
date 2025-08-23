using System.Collections.Generic;
using Godot;
using Godot.Collections;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scripts.Components;

[GlobalClass]
public partial class PopulationVisualizer : Node
{
    public enum VisualCategory { Followers, Huts }

    [Export] private Node2D _markersContainer;
    [Export] private int _unitsPerMarker = 5;
    [Export] public VisualCategory Category { get; private set; }
    [Export] private PackedScene _moddableVisualScene;
    
    private List<TierDefinition> _tiers;
    private readonly List<FollowerMarker> _markers = [];
    private long _lastKnownUnitCount = -1;
    private int _lastKnownTierIndex = -1;
    private bool _isUpdating = false;
    
    public override void _Ready()
    {
        switch (Category)
        {
            case VisualCategory.Followers:
                _tiers = GameBus.Instance.FollowerTiers;
                break;
            case VisualCategory.Huts:
                _tiers = GameBus.Instance.HutTiers;
                break;
            default:
                GD.PushError($"PopulationVisualizer has an invalid category: {Category}");
                return;
        }
        
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
        
        long currentUnitCount = Category switch
        {
            VisualCategory.Followers => (long)newState.Get(Stat.Followers),
            VisualCategory.Huts => (long)newState.Get(Stat.Followers),
            _ => 0
        };

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
                var currentVisual = marker.GetChildOrNull<ModdableVisual>(0);
                if (currentVisual == null || currentVisual.Tier != currentTier.TierEnum)
                {
                    if (marker.GetChildCount() > 0) marker.GetChild(0).QueueFree();

                    var visualInstance = _moddableVisualScene.Instantiate<ModdableVisual>();
                    visualInstance.Initialize(currentTier.TierEnum, currentTier.Texture, currentTier.Scale);
                    
                    marker.AddChild(visualInstance);
                    needsChange = true;
                }
            }
            else
            {
                if (marker.GetChildCount() > 0)
                {
                    marker.GetChild(0).QueueFree();
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
        
        GameBus.Instance.NotifyPopulationVisualsUpdated();
    }
}