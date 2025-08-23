using Godot;

namespace ParasiticGod.Scripts.Core.Effects;

[GlobalClass]
public partial class ModifyStatEffect : Effect
{
    public enum Operation { Add, Multiply }
    
    [Export] public Stat TargetStat { get; set; }
    [Export] public Operation Op { get; set; }
    [Export] public double Value { get; set; }
    
    public override void Execute(GameState gameState)
    {
        switch (Op)
        {
            case Operation.Add:
                var currentValue = gameState.Get(TargetStat);
                gameState.Set(TargetStat, currentValue + Value);
                break;
            case Operation.Multiply:
                var currentValueMul = gameState.Get(TargetStat);
                gameState.Set(TargetStat, currentValueMul * Value);
                break;
        }
    }
    
    public override string ToString()
    {
        return Op switch
        {
            Operation.Add => $"Add {Value} to {TargetStat}",
            Operation.Multiply => $"Multiply {TargetStat} by {Value}",
            _ => "Unknown Operation"
        };
    }
}