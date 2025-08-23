using Godot;

namespace ParasiticGod.Scripts;

public partial class DisableMainThemeMusic : Node
{
    public override void _Ready()
    {
        var mainThemeMusic = GetNodeOrNull<AudioStreamPlayer>("/root/MainThemeMusic");
        mainThemeMusic?.Stop();
    }
}