using System;
using System.Linq;
using ParasiticGod.Scripts.Singletons;

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
        
        var faithMultiplier = state.ActiveBuffs
            .Where(b => b.TargetStat == Stat.FaithPerFollower)
            .Aggregate(1.0f, (acc, buff) => acc * buff.Multiplier);
        
        var productionFollowerMultiplier = state.ActiveBuffs
            .Where(b => b.TargetStat == Stat.ProductionPerFollower)
            .Aggregate(1.0f, (acc, buff) => acc * buff.Multiplier);
        var productionFlatMultiplier = state.ActiveBuffs
            .Where(b => b.TargetStat == Stat.ProductionPerSecond)
            .Aggregate(1.0f, (acc, buff) => acc * buff.Multiplier);
        
        var followerMultiplier = state.ActiveBuffs
            .Where(b => b.TargetStat == Stat.FollowersPerSecond)
            .Aggregate(1.0f, (acc, buff) => acc * buff.Multiplier);
        var corruptionMultiplier = state.ActiveBuffs
            .Where(b => b.TargetStat == Stat.CorruptionPerSecond)
            .Aggregate(1.0f, (acc, buff) => acc * buff.Multiplier);
        
        var faithPerSecond = state.Get(Stat.Followers) * state.Get(Stat.FaithPerFollower) * faithMultiplier;
        
        var productionFromFollowers = state.Get(Stat.Followers) * state.Get(Stat.ProductionPerFollower) * productionFollowerMultiplier;
        var productionFromIndustry = state.Get(Stat.ProductionPerSecond) * productionFlatMultiplier;
        var totalProductionPerSecond = productionFromFollowers + productionFromIndustry;
        
        var followersPerSecond = state.Get(Stat.FollowersPerSecond) * followerMultiplier;
        var corruptionPerSecond = state.Get(Stat.CorruptionPerSecond) * corruptionMultiplier;
        
        state.Modify(Stat.Faith, faithPerSecond * delta);
        state.Modify(Stat.Production, totalProductionPerSecond * delta);
        state.Modify(Stat.Corruption, corruptionPerSecond * delta);
        state.Modify(Stat.Followers, followersPerSecond * delta);

        for (var i = state.ActiveBuffs.Count - 1; i >= 0; i--)
        {
            var buff = state.ActiveBuffs[i];
            buff.Duration -= delta;
            if (buff.Duration <= 0)
            {
                GameBus.Instance.NotifyBuffRemoved(buff);
                state.ActiveBuffs.RemoveAt(i);
                state.RemoveActiveBuff(buff.BuffId);
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