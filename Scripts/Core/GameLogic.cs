using System;

namespace ParasiticGod.Scripts.Core;

public class GameLogic
{
    public void UpdateGameState(GameState state, double delta)
    {
        state.Faith += state.FaithPerSecond * delta;

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
        if (state.Faith < miracle.FaithCost || state.Followers < miracle.FollowersRequired)
        {
            return false;
        }

        state.Faith -= miracle.FaithCost;
        
        if (miracle.Effects != null)
        {
            foreach (var effect in miracle.Effects)
            {
                effect.Execute(state);
            }
        }
        
        state.Corruption = Math.Clamp(state.Corruption, 0, 100);

        return true;
    }
}