using Godot;

namespace ParasiticGod.Scripts.Core;

[GlobalClass]
public partial class TierDefinition : Resource
{
    [Export] public PackedScene Scene { get; private set; }
    [Export] public long Threshold { get; private set; }
}