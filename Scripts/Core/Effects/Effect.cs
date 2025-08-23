using Godot;

namespace ParasiticGod.Scripts.Core.Effects;

public abstract partial class Effect : Resource
{
    public abstract void Execute(GameState gameState);

    public override string ToString()
    {
        return GetType().Name.Replace("Effect", "");
    }
}