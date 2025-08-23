using System.Collections.Generic;
using Godot;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scenes.Main;

public partial class MiraclePanel : GridContainer
{
    [Export] private PackedScene _miracleButtonScene;
    
    public override void _Ready()
    {
        GameBus.Instance.MiraclesUnlocked += OnMiraclesUnlocked;
        GameBus.Instance.MiracleCompleted += OnMiracleCompleted;
    }

    public override void _ExitTree()
    {
        GameBus.Instance.MiraclesUnlocked -= OnMiraclesUnlocked;
        GameBus.Instance.MiracleCompleted -= OnMiracleCompleted;
    }

    public void PopulateInitialButtons(Dictionary<string, MiracleDefinition> miracles)
    {
        foreach (var miracle in miracles.Values)
        {
            if (miracle.UnlockedByDefault)
            {
                GameBus.Instance.CurrentState.AddUnlockedMiracle(miracle.Id);
                AddButtonForMiracle(miracle);
            }
        }
    }

    private void OnMiraclesUnlocked(List<MiracleDefinition> miracles)
    {
        foreach (var miracle in miracles)
        {
            AddButtonForMiracle(miracle);
        }
    }

    private void OnMiracleCompleted(MiracleDefinition miracle)
    {
        foreach (var child in GetChildren())
        {
            if (child is MiracleButton button && button.GetMiracle() == miracle)
            {
                button.QueueFree();
                break;
            }
        }
    }
    
    private void AddButtonForMiracle(MiracleDefinition miracle)
    {
        var buttonInstance = _miracleButtonScene.Instantiate<MiracleButton>();
        buttonInstance.SetMiracle(miracle);
        AddChild(buttonInstance);
    }
}