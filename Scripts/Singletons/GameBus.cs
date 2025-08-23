using System;
using System.Collections.Generic;
using Godot;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Core.Effects;

namespace ParasiticGod.Scripts.Singletons;

public partial class GameBus : Node
{
    public static GameBus Instance { get; private set; }
    public Dictionary<string, MiracleDefinition> AllMiracles { get; private set; }
    public List<TierDefinition> FollowerTiers { get; private set; }
    public List<TierDefinition> HutTiers { get; private set; }

    private readonly GameState _gameState = new();
    private readonly GameLogic _gameLogic = new();

    public event Action<GameState> StateChanged;
    public event Action<MiracleDefinition> MiraclePerformed;
    public event Action<List<MiracleDefinition>> MiraclesUnlocked;
    public event Action<MiracleDefinition> MiracleCompleted;
    public event Action<Buff> BuffAdded;
    public event Action<Buff> BuffRemoved;
    public event Action PopulationVisualsUpdated;
    public event Action<string> AgeAdvanced;

    public override void _EnterTree()
    {
        Instance = this;
        AllMiracles = MiracleLoader.LoadMiraclesFromDirectory("user://Mods/Miracles");
        FollowerTiers = TierLoader.LoadTiersFromFile("user://Mods/Tiers/follower_tiers.json");
        HutTiers = TierLoader.LoadTiersFromFile("user://Mods/Tiers/hut_tiers.json");
    }

    public override void _ExitTree()
    {
        Instance = null;
    }

    public override void _Process(double delta)
    {
        _gameLogic.UpdateGameState(_gameState, delta);
        StateChanged?.Invoke(_gameState);

        if (_gameState.Get(Stat.Corruption) >= 100)
        {
            GD.Print("The world has died!");
            GetTree().Quit(); // For now
        }
    }

    public void PerformMiracle(MiracleDefinition miracle)
    {
        if (_gameLogic.TryToPerformMiracle(_gameState, miracle))
        {
            MiraclePerformed?.Invoke(miracle);
            
            var miraclesToUnlock = new List<MiracleDefinition>();
            foreach (var effect in miracle.Effects)
            {
                if (effect is UnlockMiracleEffect unlockEffect)
                {
                    foreach (var id in unlockEffect.MiraclesToUnlock)
                    {
                        if (AllMiracles.TryGetValue(id, out var def) && !_gameState.IsMiracleUnlocked(id))
                        {
                            miraclesToUnlock.Add(def);
                            _gameState.AddUnlockedMiracle(id);
                        }
                    }
                }
                else if (effect is DestroySelfEffect)
                {
                    MiracleCompleted?.Invoke(miracle);
                    _gameState.RemoveUnlockedMiracle(miracle.Id);
                }
            }

            if (miraclesToUnlock.Count > 0)
            {
                MiraclesUnlocked?.Invoke(miraclesToUnlock);
            }
            
            if (!string.IsNullOrEmpty(miracle.AdvancesToAge))
            {
                AgeAdvanced?.Invoke(miracle.AdvancesToAge);
            }
        }
    }
    
    public void NotifyPopulationVisualsUpdated()
    {
        PopulationVisualsUpdated?.Invoke();
    }
    
    public void NotifyBuffAdded(Buff buff)
    {
        BuffAdded?.Invoke(buff);
    }

    public void NotifyBuffRemoved(Buff buff)
    {
        BuffRemoved?.Invoke(buff);
    }
    
    public void SubscribeToStat(Stat stat, Action<double> listener) => _gameState.Subscribe(stat, listener);
    public void UnsubscribeFromStat(Stat stat, Action<double> listener) => _gameState.Unsubscribe(stat, listener);
    
    public GameState CurrentState => _gameState;
}