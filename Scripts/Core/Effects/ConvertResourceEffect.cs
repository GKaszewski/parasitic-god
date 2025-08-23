using Godot;

namespace ParasiticGod.Scripts.Core.Effects;

[GlobalClass]
public partial class ConvertResourceEffect : Effect
{
    [Export] public Stat FromResource { get; set; }
    [Export] public double FromAmount { get; set; }
    [Export] public Stat ToResource { get; set; }
    [Export] public double ToAmount { get; set; }
    
    public override void Execute(GameState gameState)
    {
        if (!(gameState.Get(FromResource) >= FromAmount)) return;
        
        gameState.Set(FromResource, gameState.Get(FromResource) - FromAmount);
        gameState.Set(ToResource, gameState.Get(ToResource) + ToAmount);
    }
    
    public override string ToString()
    {
        return $"Convert {FromAmount} {FromResource} to {ToAmount} {ToResource}";
    }
}