using Godot;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scripts.Core.Effects;

[GlobalClass]
public partial class ApplyBuffEffect : Effect
{
    [Export] public Stat TargetStat { get; set; }
    [Export] public float Multiplier { get; set; } = 2.0f;
    [Export] public double Duration { get; set; } = 30.0;
    
    public override void Execute(GameState gameState)
    {
        var newBuff = new Buff
        {
            Name = $"{TargetStat} x{Multiplier}",
            Multiplier = Multiplier,
            Duration = Duration
        };
        
        gameState.ActiveBuffs.Add(newBuff);
        GameBus.Instance.NotifyBuffAdded(newBuff);
    }

    public override string ToString()
    {
        return $"Apply Buff: x{Multiplier} to {TargetStat} for {Duration} seconds";
    }
}