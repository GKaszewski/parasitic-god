using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scripts;

[GlobalClass]
public partial class RoadManager : Node2D
{
    [Export] private Node2D _markersContainer;
    [Export] private float _roadWidth = 4.0f;
    [Export] private Color _roadColor = new("saddlebrown");

    [Export] private Follower.FollowerTier _minimumTierForRoads = Follower.FollowerTier.Tier2;

    private Line2D _roadNetwork;

    public override void _Ready()
    {
        _roadNetwork = new Line2D
        {
            Width = _roadWidth,
            DefaultColor = _roadColor,
        };
        AddChild(_roadNetwork);
        
        Callable.From(() =>
        {
            GenerateRoads();
            GameBus.Instance.PopulationVisualsUpdated += GenerateRoads;
        }).CallDeferred();
    }

    public override void _ExitTree()
    {
        if (GameBus.Instance != null)
        {
            GameBus.Instance.PopulationVisualsUpdated -= GenerateRoads;
        }
    }

    private void GenerateRoads()
    {
        _roadNetwork.ClearPoints();

        var activeMarkers = _markersContainer.GetChildren()
            .OfType<FollowerMarker>()
            .Where(m => m.IsOccupied && m.FollowerInstance != null &&
                        m.FollowerInstance.Tier >= _minimumTierForRoads)
            .ToList();
        
        if (activeMarkers.Count < 2) return;

        var treeNodes = new HashSet<FollowerMarker>();
        var remainingNodes = new List<FollowerMarker>(activeMarkers);
        var edges = new List<(Vector2, Vector2)>();

        var startNode = remainingNodes[0];
        treeNodes.Add(startNode);
        remainingNodes.RemoveAt(0);

        while (remainingNodes.Any())
        {
            FollowerMarker bestSource = null;
            FollowerMarker bestDest = null;
            var minDistanceSq = float.MaxValue;
            
            foreach (var source in treeNodes)
            {
                foreach (var dest in remainingNodes)
                {
                    var distSq = source.GlobalPosition.DistanceSquaredTo(dest.GlobalPosition);
                    if (distSq < minDistanceSq)
                    {
                        minDistanceSq = distSq;
                        bestSource = source;
                        bestDest = dest;
                    }
                }
            }

            if (bestDest != null)
            {
                treeNodes.Add(bestDest);
                remainingNodes.Remove(bestDest);
                edges.Add((bestSource.Position, bestDest.Position));
            }
            else
            {
                break;
            }
        }

        foreach (var (start, end) in edges)
        {
            _roadNetwork.AddPoint(start);
            _roadNetwork.AddPoint(end);
        }
    }
}