using Godot;
using Godot.Collections;
using ParasiticGod.Scripts.Core.Effects;

namespace ParasiticGod.Scripts.Core;

[GlobalClass]
public partial class MiracleDefinition : Resource
{
    public string Id { get; set; }
    public bool UnlockedByDefault { get; set; }

    [Export] public string Name { get; set; }
    [Export] public double FaithCost { get; set; }
    [Export] public long FollowersRequired { get; set; }
    [Export] public double ProductionRequired { get; set; } 
    public string AdvancesToAge { get; set; }

    [Export] public Array<Effect> Effects { get; set; }
}