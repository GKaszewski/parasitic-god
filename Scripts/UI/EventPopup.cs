using Godot;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scripts.UI;

[GlobalClass]
public partial class EventPopup : PanelContainer
{
    [Export] private Label _titleLabel;
    [Export] private RichTextLabel _descriptionLabel;
    [Export] private VBoxContainer _optionsContainer;
    [Export] private PackedScene _optionButtonScene; // A scene for a single button
    [Export] private float _choiceTimeout = 30.0f;
    
    private EventDefinition _eventDef;
    private Timer _timeoutTimer;
    private readonly RandomNumberGenerator _rng = new();
    
    public void DisplayEvent(EventDefinition eventDef)
    {
        _eventDef = eventDef;
        _titleLabel.Text = eventDef.Title;
        _descriptionLabel.Text = eventDef.Description;

        foreach (var child in _optionsContainer.GetChildren())
        {
            child.QueueFree();
        }

        foreach (var option in eventDef.Options)
        {
            var button = _optionButtonScene.Instantiate<Button>();
            button.Text = option.Text;
            button.TooltipText = option.Tooltip;
            
            button.Pressed += () =>
            {
                _timeoutTimer.Stop();
                HandleChoice(option);
            };
            
            _optionsContainer.AddChild(button);
        }
        
        _timeoutTimer = new Timer { WaitTime = _choiceTimeout, OneShot = true };
        AddChild(_timeoutTimer);
        _timeoutTimer.Timeout += OnTimeout;
        _timeoutTimer.Start();
    }
    
    private void OnTimeout()
    {
        if (_eventDef.Options.Count > 0)
        {
            var randomIndex = _rng.RandiRange(0, _eventDef.Options.Count - 1);
            HandleChoice(_eventDef.Options[randomIndex]);
        }
        else
        {
            QueueFree();
            GetTree().Paused = false;
        }
    }

    private void HandleChoice(EventOptionDefinition option)
    {
        GameBus.Instance.ExecuteEffects(option.Effects);
        QueueFree();
    }
}