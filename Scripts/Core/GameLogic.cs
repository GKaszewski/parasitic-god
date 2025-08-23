using System;

namespace ParasiticGod.Scripts.Core;

public class GameLogic
{
    public void UpdateGameState(GameState state, double delta)
    {
        var totalMultiplier = 1.0;
        foreach (var buff in state.ActiveBuffs)
        {
            totalMultiplier *= buff.Multiplier;
        }
        var faithPerSecond = state.Get(Stat.Followers) * state.Get(Stat.FaithPerFollower) * totalMultiplier;
        
        state.Modify(Stat.Faith, faithPerSecond * delta);
        state.Modify(Stat.Production, state.Get(Stat.ProductionPerSecond) * delta);
        state.Modify(Stat.Corruption, state.Get(Stat.CorruptionPerSecond) * delta);

        for (var i = state.ActiveBuffs.Count - 1; i >= 0; i--)
        {
            var buff = state.ActiveBuffs[i];
            buff.Duration -= delta;
            if (buff.Duration <= 0)
            {
                state.ActiveBuffs.RemoveAt(i);
            }
        }
    }

    public bool TryToPerformMiracle(GameState state, MiracleDefinition miracle)
    {
        if (state.Get(Stat.Faith) < miracle.FaithCost || 
            state.Get(Stat.Followers) < miracle.FollowersRequired ||
            state.Get(Stat.Production) < miracle.ProductionRequired)
        {
            return false;
        }

        state.Modify(Stat.Faith, -miracle.FaithCost);
        state.Modify(Stat.Production, -miracle.ProductionRequired);
    
        if (miracle.Effects != null)
        {
            foreach (var effect in miracle.Effects)
            {
                effect.Execute(state);
            }
        }
    
        state.Set(Stat.Corruption, Math.Clamp(state.Get(Stat.Corruption), 0, 100));
        return true;
    }
}