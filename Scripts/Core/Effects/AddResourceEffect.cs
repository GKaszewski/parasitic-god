using System;
using Godot;

namespace ParasiticGod.Scripts.Core.Effects;

[GlobalClass]
public partial class AddResourceEffect : Effect
{
    [Export] public Stat TargetResource { get; set; }
    [Export] public double Value { get; set; }

    public override void Execute(GameState state)
    {
        state.Modify(TargetResource, Value);
    }

    public override string ToString()
    {
        return $"Add {Value} to {TargetResource}";
    }
}