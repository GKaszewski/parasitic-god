using System;
using System.Collections.Generic;
using ParasiticGod.Scripts.Core.Effects;

namespace ParasiticGod.Scripts.Core;

public class GameState
{
    private readonly Dictionary<Stat, StatData> _stats = new();
    private readonly Dictionary<string, MiracleDefinition> _miraclesInHand = new();
    
    public List<Buff> ActiveBuffs { get; } = [];
    
    public GameState()
    {
        // Initialize all stats with their default values
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            _stats[stat] = new StatData();
        }
        
        Set(Stat.Faith, 50);
        Set(Stat.Followers, 40);
        Set(Stat.FaithPerFollower, 0.5);
        Set(Stat.ProductionPerSecond, 0.0);
        Set(Stat.CorruptionPerSecond, 0.01);
    }
    
    public double Get(Stat stat) => _stats[stat].Value;
    
    public void Set(Stat stat, double value) => _stats[stat].Set(value);
    
    public void Modify(Stat stat, double delta) => _stats[stat].Set(Get(stat) + delta);
    
    public void Subscribe(Stat stat, Action<double> listener) => _stats[stat].OnChanged += listener;
    
    public void Unsubscribe(Stat stat, Action<double> listener) => _stats[stat].OnChanged -= listener;
    
    public Dictionary<string, MiracleDefinition> MiraclesInHand() => _miraclesInHand;
}