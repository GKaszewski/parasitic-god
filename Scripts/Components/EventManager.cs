using System.Collections.Generic;
using Godot;
using Limbo.Console.Sharp;
using ParasiticGod.Scripts.Core;
using ParasiticGod.Scripts.Singletons;
using ParasiticGod.Scripts.UI;

namespace ParasiticGod.Scripts.Components;

[GlobalClass]
public partial class EventManager : Node
{
    [Export] private double _checkInterval = 5.0;
    [Export] private PackedScene _eventPopupScene;
    [Export] private Container _eventPopupContainer;
    
    private List<EventDefinition> _allEvents;
    private Timer _timer;
    private RandomNumberGenerator _rng = new();
    
    public override void _Ready()
    {
        RegisterConsoleCommands();
        _allEvents = GameBus.Instance.AllEvents;
        
        _timer = new Timer { WaitTime = _checkInterval, Autostart = true };
        AddChild(_timer);
        _timer.Timeout += OnCheckEvents;
    }
    
    private void OnCheckEvents()
    {
        if (GetTree().Paused) return;
        
        var state = GameBus.Instance.CurrentState;
        
        foreach (var ev in _allEvents)
        {
            if (state.Get(Stat.Followers) < ev.Trigger.MinFollowers) continue;
            if (state.Get(Stat.Corruption) > ev.Trigger.MaxCorruption) continue;

            var probability = _checkInterval / ev.MeanTimeToHappen;
            if (_rng.Randf() < probability)
            {
                FireEvent(ev);
                break;
            }
        }
    }
    
    private void FireEvent(EventDefinition eventDef)
    {
        GD.Print($"Firing event: {eventDef.Title}");
        GetTree().Paused = true;

        var popup = _eventPopupScene.Instantiate<EventPopup>();
        _eventPopupContainer.AddChild(popup);
        popup.DisplayEvent(eventDef);
    }
    
    [ConsoleCommand("trigger_event", "Triggers an event by its ID for testing purposes.")]
    private void TriggerEventCommand(string eventId)
    {
        var eventDef = _allEvents.Find(e => e.Id == eventId);
        if (eventDef != null)
        {
            FireEvent(eventDef);
        }
        else
        {
            GD.PushError($"No event found with ID: {eventId}");
        }
    }
}