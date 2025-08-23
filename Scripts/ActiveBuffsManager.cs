using System;
using System.Collections.Generic;
using Godot;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Core.Effects;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scripts;

[GlobalClass]
public partial class ActiveBuffsManager : Node
{
    [Export] private PackedScene _activeBuffScene;
    
    private readonly Dictionary<Guid, ActiveBuffUi> _activeBuffUis = new();
    
    public override void _Ready()
    {
        GameBus.Instance.BuffAdded += OnBuffAdded;
        GameBus.Instance.BuffRemoved += OnBuffRemoved;
    }

    public override void _ExitTree()
    {
        if (GameBus.Instance == null) return;
        GameBus.Instance.BuffAdded -= OnBuffAdded;
        GameBus.Instance.BuffRemoved -= OnBuffRemoved;
    }

    private void OnBuffAdded(Buff buff)
    {
        var buffInstance = _activeBuffScene.Instantiate<ActiveBuffUi>();
        AddChild(buffInstance);
        buffInstance.SetBuff(buff);
        _activeBuffUis.Add(buff.Id, buffInstance);
    }

    private void OnBuffRemoved(Buff buff)
    {
        if (_activeBuffUis.TryGetValue(buff.Id, out var buffUi))
        {
            buffUi.QueueFree();
            _activeBuffUis.Remove(buff.Id);
        }
    }
}