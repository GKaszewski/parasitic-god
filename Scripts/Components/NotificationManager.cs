using Godot;
using ParasiticGod.Scripts;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scripts.Components;

[GlobalClass]
public partial class NotificationManager : CanvasLayer
{
    [Export] private PackedScene _notificationLabelScene;
    [Export] private AudioStreamPlayer _sfx;
    
    public override void _Ready()
    {
        GameBus.Instance.AgeAdvanced += OnAgeAdvanced;
    }

    public override void _ExitTree()
    {
        if (GameBus.Instance != null)
        {
            GameBus.Instance.AgeAdvanced -= OnAgeAdvanced;
        }
    }

    private void OnAgeAdvanced(string ageName)
    {
        var notification = _notificationLabelScene.Instantiate<NotificationLabel>();
        AddChild(notification);
        _sfx?.Play();
        notification.ShowNotification($"You have entered\n{ageName}!");
    }
}