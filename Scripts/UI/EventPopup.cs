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
    
    public void DisplayEvent(EventDefinition eventDef)
    {
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
                GameBus.Instance.ExecuteEffects(option.Effects);
                QueueFree();
            };
            
            _optionsContainer.AddChild(button);
        }
    }
}