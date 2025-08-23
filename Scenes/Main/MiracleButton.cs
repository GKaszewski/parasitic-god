using System.Collections.Generic;
using System.Linq;
using Godot;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scenes.Main;

public partial class MiracleButton : Button
{
    private MiracleDefinition _miracle;
    [Export] private AudioStreamPlayer _sfx;

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

        if (_sfx == null)
        {
            _sfx = GetNodeOrNull<AudioStreamPlayer>("SFX");
        }
        
        Pressed += OnPressed;
        
        GameBus.Instance.StateChanged += UpdateAvailability;
        UpdateAvailability(GameBus.Instance.CurrentState);
    }

    public override void _ExitTree()
    {
        Pressed -= OnPressed;
        
        if (GameBus.Instance != null)
        {
            GameBus.Instance.StateChanged -= UpdateAvailability;
        }
    }

    private void OnPressed()
    {
        _sfx?.Play();
        GameBus.Instance.PerformMiracle(_miracle);
    }

    public void SetMiracle(MiracleDefinition miracle)
    {
        _miracle = miracle;
    }

    public MiracleDefinition GetMiracle() { return _miracle; }
    
    /// <summary>
    /// Checks the miracle's requirements against the current game state
    /// and updates the button's disabled status and tooltip.
    /// </summary>
    private void UpdateAvailability(GameState state)
    {
        if (_miracle == null) return;

        var missingRequirements = new List<string>();

        if (state.Get(Stat.Faith) < _miracle.FaithCost)
        {
            missingRequirements.Add($"Not enough Faith ({state.Get(Stat.Faith):F0} / {_miracle.FaithCost})");
        }
        if (state.Get(Stat.Followers) < _miracle.FollowersRequired)
        {
            missingRequirements.Add($"Not enough Followers ({(long)state.Get(Stat.Followers)} / {_miracle.FollowersRequired})");
        }
        if (state.Get(Stat.Production) < _miracle.ProductionRequired)
        {
            missingRequirements.Add($"Not enough Production ({state.Get(Stat.Production):F0} / {_miracle.ProductionRequired})");
        }

        if (missingRequirements.Any())
        {
            Disabled = true;
            TooltipText = string.Join("\n", missingRequirements);
        }
        else
        {
            Disabled = false;
            TooltipText = BuildTooltipText();
        }
    }
    
    private string BuildText()
    {
        string costText;
        if (_miracle.ProductionRequired > 0 && _miracle.FaithCost <= 0)
        {
            costText = $"Cost: {_miracle.ProductionRequired:F0} Prod";
        }
        else
        {
            costText = $"Cost: {_miracle.FaithCost:F0} Faith";
        }
        
        return $"{_miracle.Name}\n{costText}";
    }
    
    private string BuildTooltipText()
    {
        var tooltip = "";
        
        if (_miracle.FaithCost > 0)
            tooltip += $"Cost: {_miracle.FaithCost:F0} Faith\n";
        if (_miracle.ProductionRequired > 0)
            tooltip += $"Cost: {_miracle.ProductionRequired:F0} Production\n";
        if (_miracle.FollowersRequired > 0)
            tooltip += $"Requires: {_miracle.FollowersRequired} Followers\n";
        
        tooltip += "\nEffects:\n";
        
        foreach (var effect in _miracle.Effects)
        {
            if (string.IsNullOrEmpty(effect.ToString())) continue;
            tooltip += $"- {effect}\n";
        }
        return tooltip.TrimEnd();
    }
}