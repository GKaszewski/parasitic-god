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
        
        Text = BuildText();
        TooltipText = BuildTooltipText();
        
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
        Text = BuildText();
        TooltipText = BuildTooltipText();
    }

    public MiracleDefinition GetMiracle() { return _miracle; }
    
    private string BuildText()
    {
        return $"{_miracle.Name}\nCost: {_miracle.FaithCost} Faith";
    }
    
    private string BuildTooltipText()
    {
        var tooltip = $"Cost: {_miracle.FaithCost} Faith\nRequires: {_miracle.FollowersRequired} Followers\nEffects:\n";
        foreach (var effect in _miracle.Effects)
        {
            if (effect.ToString() == string.Empty) continue;
            tooltip += $"- {effect}\n";
        }
        return tooltip.TrimEnd();
    }
}