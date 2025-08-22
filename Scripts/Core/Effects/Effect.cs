using Godot;

namespace ParasiticGod.Scripts.Core.Effects;

public abstract partial class Effect : Resource
{
    public abstract void Execute(GameState gameState);
}