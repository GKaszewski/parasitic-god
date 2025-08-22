using Godot;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scenes.Main;

public partial class MiracleButton : Button
{
    private MiracleDefinition _miracle;

    public override void _Ready()
    {
        if (_miracle == null)
        {
            GD.PrintErr($"MiracleButton '{Name}' has no MiracleDefinition assigned.");
            SetProcess(false);
            return;
        }
        
        Text = $"{_miracle.Name}\nCost: {_miracle.FaithCost} Faith";
        
        Pressed += OnPressed;
    }

    public override void _ExitTree()
    {
        Pressed -= OnPressed;
    }

    private void OnPressed()
    {
        GameBus.Instance.PerformMiracle(_miracle);
    }

    public void SetMiracle(MiracleDefinition miracle)
    {
        _miracle = miracle;
        Text = $"{_miracle.Name}\nCost: {_miracle.FaithCost} Faith";
    }

    public MiracleDefinition GetMiracle() { return _miracle; }
}