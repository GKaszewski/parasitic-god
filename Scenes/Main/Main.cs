using Godot;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scenes.Main;

public partial class Main : Node
{
    [Export] private Label _faithLabel;
    [Export] private Label _followersLabel;
    [Export] private Label _corruptionLabel;
    [Export] private Label _productionLabel;
    [Export] private MiraclePanel _miraclePanel;
    [Export] private Sprite2D _worldSprite;
    [Export] private Color _deadWorldColor = new("#581845");

    public override void _Ready()
    {
        GameBus.Instance.SubscribeToStat(Stat.Faith, UpdateFaithLabel);
        GameBus.Instance.SubscribeToStat(Stat.Followers, UpdateFollowersLabel);
        GameBus.Instance.SubscribeToStat(Stat.Corruption, UpdateCorruptionLabel);
        GameBus.Instance.SubscribeToStat(Stat.Production, UpdateProductionLabel);
        
        _miraclePanel.PopulateInitialButtons(GameBus.Instance.AllMiracles);
        
        GameBus.Instance.StateChanged += OnStateChanged;
        
        UpdateFaithLabel(GameBus.Instance.CurrentState.Get(Stat.Faith));
        UpdateFollowersLabel(GameBus.Instance.CurrentState.Get(Stat.Followers));
        UpdateCorruptionLabel(GameBus.Instance.CurrentState.Get(Stat.Corruption));
        UpdateProductionLabel(GameBus.Instance.CurrentState.Get(Stat.Production));
    }

    public override void _ExitTree()
    {
        GameBus.Instance.UnsubscribeFromStat(Stat.Faith, UpdateFaithLabel);
        GameBus.Instance.UnsubscribeFromStat(Stat.Followers, UpdateFollowersLabel);
        GameBus.Instance.UnsubscribeFromStat(Stat.Corruption, UpdateCorruptionLabel);
        GameBus.Instance.UnsubscribeFromStat(Stat.Production, UpdateProductionLabel);
        GameBus.Instance.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState newState)
    {
        UpdateWorldVisuals(newState.Get(Stat.Corruption));
    }

    private void UpdateWorldVisuals(double corruption)
    {
        if (_worldSprite.Material is not ShaderMaterial shaderMaterial) return;
        
        var ratio = (float)corruption / 100.0f;
        shaderMaterial.SetShaderParameter("corruption_level", ratio);
    }
    
    private void UpdateFaithLabel(double newValue)
    {
        _faithLabel.Text = $"Faith: {newValue:F0}";
    }
    
    private void UpdateFollowersLabel(double newValue)
    {
        _followersLabel.Text = $"Followers: {newValue:F0}";
    }
    
    private void UpdateCorruptionLabel(double newValue)
    {
        _corruptionLabel.Text = $"Corruption: {newValue:F0}%";
    }
    
    private void UpdateProductionLabel(double production)
    {
        _productionLabel.Text = $"Production: {production:F0}";
    }
}