using Godot;

namespace ParasiticGod.Scripts;

public partial class EnableMainThemeMusic : Node
{
    public override void _Ready()
    {
        var mainThemeMusic = GetNodeOrNull<AudioStreamPlayer>("/root/MainThemeMusic");
        if (mainThemeMusic is { Playing: false })
        {
            mainThemeMusic.Play();
        }
    }
}