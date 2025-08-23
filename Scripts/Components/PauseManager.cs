using Godot;

namespace ParasiticGod.Scripts.Components;

[GlobalClass]
public partial class PauseManager : CanvasLayer
{
    [Export] private Button _pauseButton;
    [Export] private Control _pauseMenu;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        _pauseMenu.Hide();
        _pauseButton.Pressed += TogglePause;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause")) TogglePause();
    }

    private void TogglePause()
    {
        var isPaused = !GetTree().Paused;
        GetTree().Paused = isPaused;
        _pauseMenu.Visible = isPaused;
    }
}