using Godot;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scenes.Main;

public partial class Main : Node
{
    [Export] private Label _faithLabel;
    [Export] private Label _followersLabel;
    [Export] private Label _corruptionLabel;
    [Export] private MiraclePanel _miraclePanel;
    [Export] private Sprite2D _worldSprite;
    [Export] private Color _deadWorldColor = new Color("#581845");

    public override void _Ready()
    {
        GameBus.Instance.StateChanged += OnStateChanged;
        
        _miraclePanel.PopulateInitialButtons(GameBus.Instance.AllMiracles);
    }

    public override void _ExitTree()
    {
        GameBus.Instance.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState newState)
    {
        _faithLabel.Text = $"Faith: {newState.Faith:F0} (+{newState.FaithPerSecond:F1}/s)";
        _followersLabel.Text = $"Followers: {newState.Followers}";
        _corruptionLabel.Text = $"Corruption: {newState.Corruption:F0}%";

        UpdateWorldVisuals(newState.Corruption);
    }

    private void UpdateWorldVisuals(double corruption)
    {
        if (_worldSprite.Material is not ShaderMaterial shaderMaterial) return;
        
        var ratio = (float)corruption / 100.0f;
        shaderMaterial.SetShaderParameter("corruption_level", ratio);
    }
}