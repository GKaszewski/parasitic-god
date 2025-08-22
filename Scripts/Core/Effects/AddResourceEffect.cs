using System;
using Godot;

namespace ParasiticGod.Scripts.Core.Effects;

[GlobalClass]
public partial class AddResourceEffect : Effect
{
    [Export] public ResourceType TargetResource { get; set; }
    [Export] public double Value { get; set; }

    public override void Execute(GameState state)
    {
        switch (TargetResource)
        {
            case ResourceType.Faith:
                state.Faith += Value;
                break;
            case ResourceType.Followers:
                state.Followers += (long)Value;
                break;
            case ResourceType.Corruption:
                state.Corruption += Value;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}