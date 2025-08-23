using Godot;

namespace ParasiticGod.Scripts;

public partial class MainMenu : Node
{
    [Export] private PackedScene _gameScene;
    [Export] private Button _startButton;
    [Export] private Button _quitButton;

    public override void _Ready()
    {
        if (_startButton != null)
        {
            _startButton.Pressed += OnStartButtonPressed;
        }

        if (_quitButton != null)
        {
            _quitButton.Pressed += OnQuitButtonPressed;
        }
    }

    public override void _ExitTree()
    {
        if (_startButton != null)
        {
            _startButton.Pressed -= OnStartButtonPressed;
        }

        if (_quitButton != null)
        {
            _quitButton.Pressed -= OnQuitButtonPressed;
        }
    }

    private void OnStartButtonPressed()
    {
        if (_gameScene == null)
        {
            GD.PrintErr("Game scene is not assigned in MainMenu.");
            return;
        }

        GetTree().ChangeSceneToPacked(_gameScene);
    }
    
    private void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}