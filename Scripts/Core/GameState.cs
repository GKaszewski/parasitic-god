using System;
using System.Collections.Generic;
using ParasiticGod.Scripts.Core.Effects;
using static System.Double;

namespace ParasiticGod.Scripts.Core;

public class GameState
{
    private readonly Dictionary<Stat, StatData> _stats = new();
    private readonly HashSet<string> _unlockedMiracleIds = [];
    private readonly HashSet<string> _activeBuffIds = [];
    
    public List<Buff> ActiveBuffs { get; } = [];
    
    public GameState()
    {
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            _stats[stat] = new StatData();
        }
        
        Set(Stat.Faith, 50);
        Set(Stat.Followers, 40);
        Set(Stat.FaithPerFollower, 0.5);
        Set(Stat.ProductionPerSecond, 0.0);
        Set(Stat.CorruptionPerSecond, 0.01);
        Set(Stat.FollowersPerSecond, 0);
        Set(Stat.ProductionPerFollower, 0);
    }
    
    public double Get(Stat stat) => _stats[stat].Value;
    
    public void Set(Stat stat, double value) => _stats[stat].Set(Math.Clamp(value, 0, MaxValue));
    
    public void Modify(Stat stat, double delta) => _stats[stat].Set(Get(stat) + delta);
    
    public void Subscribe(Stat stat, Action<double> listener) => _stats[stat].OnChanged += listener;
    
    public void Unsubscribe(Stat stat, Action<double> listener) => _stats[stat].OnChanged -= listener;
    
    public bool IsMiracleUnlocked(string miracleId) => _unlockedMiracleIds.Contains(miracleId);
    public void AddUnlockedMiracle(string miracleId) => _unlockedMiracleIds.Add(miracleId);
    public void RemoveUnlockedMiracle(string miracleId) => _unlockedMiracleIds.Remove(miracleId);
    
    public bool IsBuffActive(string buffId) => _activeBuffIds.Contains(buffId);
    public void AddActiveBuff(string buffId) => _activeBuffIds.Add(buffId);
    public void RemoveActiveBuff(string buffId) => _activeBuffIds.Remove(buffId);
}