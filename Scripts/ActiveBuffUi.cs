using Godot;
using ParasiticGod.Scripts.Core.Effects;

namespace ParasiticGod.Scripts;

[GlobalClass]
public partial class ActiveBuffUi : Button
{
    private Buff _buff;

    public void SetBuff(Buff buff)
    {
        _buff = buff;
        Disabled = true;
        UpdateDisplay();
    }

    public override void _Process(double delta)
    {
        if (_buff != null)
        {
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        Text = _buff.Name;
        TooltipText = $"x{_buff.Multiplier:F1} to {_buff.Name.Split(' ')[0]}\n{_buff.Duration:F0}s remaining";
    }
}