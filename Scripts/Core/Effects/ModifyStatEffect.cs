using Godot;

namespace ParasiticGod.Scripts.Core.Effects;

[GlobalClass]
public partial class ModifyStatEffect : Effect
{
    public enum Stat { FaithPerFollower }
    public enum Operation { Add, Multiply }
    
    [Export] public Stat TargetStat { get; set; }
    [Export] public Operation Op { get; set; }
    [Export] public double Value { get; set; }
    
    public override void Execute(GameState gameState)
    {
        if (TargetStat == Stat.FaithPerFollower)
        {
            switch (Op)
            {
                case Operation.Add:
                    gameState.FaithPerFollower += Value;
                    break;
                case Operation.Multiply:
                    gameState.FaithPerFollower *= Value;
                    break;
            }
        }
    }
}