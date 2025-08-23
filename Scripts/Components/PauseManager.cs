using Godot;
using ParasiticGod.Scripts.Singletons;

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
        
        GameBus.Instance.PauseStateChanged += OnPauseStateChanged;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause")) TogglePause();
    }
    
    public override void _ExitTree()
    {
        if (GameBus.Instance != null)
        {
            GameBus.Instance.PauseStateChanged -= OnPauseStateChanged;
        }
    }

    private void TogglePause()
    {
        GameBus.Instance.SetPause(!GetTree().Paused);
    }
    
    private void OnPauseStateChanged(bool isPaused)
    {
        _pauseMenu.Visible = isPaused;
    }
}