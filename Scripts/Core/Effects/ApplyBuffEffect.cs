using Godot;

namespace ParasiticGod.Scripts.Core.Effects;

[GlobalClass]
public partial class ApplyBuffEffect : Effect
{
    public enum BuffTarget { FaithGeneration }
    
    [Export] public BuffTarget TargetStat { get; set; }
    [Export] public float Multiplier { get; set; } = 2.0f;
    [Export] public double Duration { get; set; } = 30.0;
    
    public override void Execute(GameState gameState)
    {
        var newBuff = new Buff
        {
            Multiplier = Multiplier,
            Duration = Duration
        };
        
        gameState.ActiveBuffs.Add(newBuff);
    }
}