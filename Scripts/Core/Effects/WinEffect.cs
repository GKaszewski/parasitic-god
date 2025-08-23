using Godot;
using ParasiticGod.Scripts.Singletons;

namespace ParasiticGod.Scripts.Core.Effects;

[GlobalClass]
public partial class WinEffect : Effect
{
    public override void Execute(GameState gameState)
    {
        GameBus.Instance.NotifyGameIsWon();
    }
}