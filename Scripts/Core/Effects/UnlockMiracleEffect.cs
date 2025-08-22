using Godot;
using Godot.Collections;

namespace ParasiticGod.Scripts.Core.Effects;

[GlobalClass]
public partial class UnlockMiracleEffect : Effect
{
    [Export]
    public Array<string> MiraclesToUnlock { get; set; }
    
    public override void Execute(GameState gameState)
    {
        
    }
}