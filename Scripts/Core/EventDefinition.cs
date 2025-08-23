using System.Collections.Generic;
using Godot.Collections;
using ParasiticGod.Scripts.Core.Effects;

namespace ParasiticGod.Scripts.Core;

public class EventDefinition
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int MeanTimeToHappen { get; set; }
    public EventTriggerDto Trigger { get; set; }
    public List<EventOptionDefinition> Options { get; set; } = [];
}

public class EventOptionDefinition
{
    public string Text { get; set; }
    public string Tooltip { get; set; }
    public Array<Effect> Effects { get; set; }
}