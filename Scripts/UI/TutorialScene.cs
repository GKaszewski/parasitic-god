using DialogueManagerRuntime;
using Godot;

namespace ParasiticGod.Scripts.UI;

public partial class TutorialScene : Control
{
    [Export] private PackedScene _mainGameScene;
    [Export] private Resource _tutorialDialogue;
    
    public override void _Ready()
    {
        DialogueManager.DialogueEnded += OnDialogueEnded;
        DialogueManager.ShowExampleDialogueBalloon(_tutorialDialogue, "start");
    }
    
    public override void _ExitTree()
    {
        DialogueManager.DialogueEnded -= OnDialogueEnded;
    }
    
    private void OnDialogueEnded(Resource resource)
    {
        if (resource == _tutorialDialogue)
        {
            GetTree().ChangeSceneToPacked(_mainGameScene);
        }
    }
}