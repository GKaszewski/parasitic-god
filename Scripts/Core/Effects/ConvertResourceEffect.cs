using Godot;

namespace ParasiticGod.Scripts.Core.Effects;

[GlobalClass]
public partial class ConvertResourceEffect : Effect
{
    [Export] public ResourceType FromResource { get; set; }
    [Export] public double FromAmount { get; set; }
    [Export] public ResourceType ToResource { get; set; }
    [Export] public double ToAmount { get; set; }
    
    public override void Execute(GameState gameState)
    {
        double GetValue(ResourceType type) => type switch {
            ResourceType.Faith => gameState.Faith,
            ResourceType.Followers => gameState.Followers,
            ResourceType.Corruption => gameState.Corruption,
            _ => 0
        };
        
        void SetValue(ResourceType type, double value) {
            switch(type) {
                case ResourceType.Faith: gameState.Faith = value; break;
                case ResourceType.Followers: gameState.Followers = (long)value; break;
                case ResourceType.Corruption: gameState.Corruption = value; break;
            }
        }

        if (GetValue(FromResource) >= FromAmount)
        {
            SetValue(FromResource, GetValue(FromResource) - FromAmount);
            SetValue(ToResource, GetValue(ToResource) + ToAmount);
        }
    }
}