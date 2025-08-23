using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scripts.Components;

[GlobalClass]
public partial class ForestVisualizer : Node
{
    [Export] private Node2D _treesContainer;
    
    private List<Node2D> _trees = [];
    private int _lastKnownTreesToShow = -1;
    private bool _isUpdating = false;
    
    public override void _Ready()
    {
        foreach (var child in _treesContainer.GetChildren())
        {
            if (child is Node2D tree)
            {
                _trees.Add(tree);
            }
        }

        var rng = new RandomNumberGenerator();
        rng.Randomize();
        _trees = _trees.OrderBy(_ => Guid.NewGuid()).ToList();

        GameBus.Instance.StateChanged += OnStateChanged;
    }

    public override void _ExitTree()
    {
        GameBus.Instance.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState newState)
    {
        if (_isUpdating) return;
        
        var corruptionRatio = newState.Get(Stat.Corruption) / 100.0;
        var treesToShow = (int)(_trees.Count * (1.0 - corruptionRatio));

        if (treesToShow != _lastKnownTreesToShow)
        {
            UpdateForestProgressively(treesToShow);
        }
    }

    private async void UpdateForestProgressively(int treesToShow)
    {
        _isUpdating = true;

        for (var i = 0; i < _trees.Count; i++)
        {
            var tree = _trees[i];
            var shouldBeVisible = i < treesToShow;
            var needsChange = tree.Visible != shouldBeVisible;

            if (needsChange)
            {
                tree.Visible = shouldBeVisible;
                await ToSignal(GetTree().CreateTimer(0.01f), SceneTreeTimer.SignalName.Timeout);
            }
        }

        _lastKnownTreesToShow = treesToShow;
        _isUpdating = false;
    }
}