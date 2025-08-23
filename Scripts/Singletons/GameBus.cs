using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Limbo.Console.Sharp;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Core.Effects;

namespace ParasiticGod.Scripts.Singletons;

public partial class GameBus : Node
{
    public static GameBus Instance { get; private set; }
    public System.Collections.Generic.Dictionary<string, MiracleDefinition> AllMiracles { get; private set; }
    public List<TierDefinition> FollowerTiers { get; private set; }
    public List<TierDefinition> HutTiers { get; private set; }
    public List<TierDefinition> TempleTiers { get; private set; }
    public List<EventDefinition> AllEvents { get; private set; }

    private PackedScene _gameOverScene = GD.Load<PackedScene>("res://Scenes/game_over.tscn");
    private PackedScene _winScene = GD.Load<PackedScene>("res://Scenes/win_screen.tscn");

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
    public event Action GameWon;
    public event Action<bool> PauseStateChanged;

    public override void _EnterTree()
    {
        Instance = this;
        AllMiracles = MiracleLoader.LoadAllMiracles();
        AllEvents = EventLoader.LoadAllEvents();
        FollowerTiers = TierLoader.LoadTiers("res://Mods/Tiers/follower_tiers.json", "user://Mods/Tiers/follower_tiers.json");
        HutTiers = TierLoader.LoadTiers("res://Mods/Tiers/hut_tiers.json","user://Mods/Tiers/hut_tiers.json");
        TempleTiers = TierLoader.LoadTiers("res://Mods/Tiers/temple_tiers.json","user://Mods/Tiers/temple_tiers.json");
        
        GameWon += OnGameWon;
    }

    public override void _ExitTree()
    {
        Instance = null;
        GameWon -= OnGameWon;
    }

    public override void _Ready()
    {
        RegisterConsoleCommands();
    }

    public override void _Process(double delta)
    {
        _gameLogic.UpdateGameState(_gameState, delta);
        StateChanged?.Invoke(_gameState);
        
        if (_gameState.Get(Stat.Corruption) >= 100)
        {
            GetTree().ChangeSceneToPacked(_gameOverScene);
            _gameState.Set(Stat.Corruption, 0);
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
                            _gameState.AddUnlockedMiracle(def.Id);
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
    
    public void NotifyGameIsWon()
    {
        GameWon?.Invoke();
    }
    
    public void ExecuteEffects(Array<Effect> effects)
    {
        foreach (var effect in effects)
        {
            effect.Execute(_gameState);
        }
        SetPause(false);
    }
    
    public void SubscribeToStat(Stat stat, Action<double> listener) => _gameState.Subscribe(stat, listener);
    public void UnsubscribeFromStat(Stat stat, Action<double> listener) => _gameState.Unsubscribe(stat, listener);
    
    public void SetPause(bool isPaused)
    {
        GetTree().Paused = isPaused;
        PauseStateChanged?.Invoke(isPaused);
    }
    
    public GameState CurrentState => _gameState;
    
    [ConsoleCommand("set_stat", "Sets the value of a specified stat.")]
    private void SetStatCommand(Stat stat, double value) => _gameState.Set(stat, value);
    
    [ConsoleCommand("game_over")]
    private void GameOverCommand() => GetTree().ChangeSceneToPacked(_gameOverScene);
    
    [ConsoleCommand("win_game")]
    private void WinGameCommand() => GetTree().ChangeSceneToPacked(_winScene);
    
    private void OnGameWon()
    {
        GetTree().ChangeSceneToPacked(_winScene);
    }
}