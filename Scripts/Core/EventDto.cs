using System.Collections.Generic;
using Newtonsoft.Json;

namespace ParasiticGod.Scripts.Core;

public class EventTriggerDto
{
    [JsonProperty("minFollowers")]
    public long MinFollowers { get; set; } = 0;
    
    [JsonProperty("maxCorruption")]
    public double MaxCorruption { get; set; } = 100;
}

public class EventOptionDto
{
    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("tooltip")]
    public string Tooltip { get; set; }

    [JsonProperty("effects")]
    public List<EffectDto> Effects { get; set; }
}

public class EventDto
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("meanTimeToHappen")]
    public int MeanTimeToHappen { get; set; } = 100; // in game days

    [JsonProperty("trigger")]
    public EventTriggerDto Trigger { get; set; }

    [JsonProperty("options")]
    public List<EventOptionDto> Options { get; set; }
}