using Godot;

namespace ParasiticGod.Scripts;

[GlobalClass]
public partial class NotificationLabel : Label
{
    public void ShowNotification(string text)
    {
        Text = text;
        PivotOffset = Size / 2;
        GlobalPosition = GetViewportRect().Size / 2;

        var tween = CreateTween();
        tween.TweenProperty(this, "modulate:a", 1.0f, 0.5f).From(0.0f);
        tween.TweenInterval(2.5f);
        tween.TweenProperty(this, "modulate:a", 0.0f, 1.0f);
        tween.Finished += QueueFree;
    }
}