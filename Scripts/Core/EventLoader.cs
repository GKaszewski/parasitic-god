using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

namespace ParasiticGod.Scripts.Core;

public static class EventLoader
{
    public static List<EventDefinition> LoadAllEvents()
    {
        var loadedEvents = new Dictionary<string, EventDefinition>();
        
        LoadEventsFromPath("res://Mods/Events", loadedEvents);
        LoadEventsFromPath("user://Mods/Events", loadedEvents);

        GD.Print($"Finished loading. Total unique events: {loadedEvents.Count}");
        return new List<EventDefinition>(loadedEvents.Values);
    }
    
    private static void LoadEventsFromPath(string path, Dictionary<string, EventDefinition> events)
    {
        if (!DirAccess.DirExistsAbsolute(path)) return;

        using var dir = DirAccess.Open(path);
        dir.ListDirBegin();
        var fileName = dir.GetNext();
        while (!string.IsNullOrEmpty(fileName))
        {
            if (!dir.CurrentIsDir() && fileName.EndsWith(".json"))
            {
                var filePath = path.PathJoin(fileName);
                var ev = LoadEventFromFile(filePath);
                if (ev != null)
                {
                    events[ev.Id] = ev; // Add or overwrite
                }
            }
            fileName = dir.GetNext();
        }
    }
    
    private static EventDefinition LoadEventFromFile(string filePath)
    {
        var fileContent = FileAccess.GetFileAsString(filePath);
        if (string.IsNullOrEmpty(fileContent)) return null;

        var dto = JsonConvert.DeserializeObject<EventDto>(fileContent);
        if (dto == null) return null;

        var eventDef = new EventDefinition
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            MeanTimeToHappen = dto.MeanTimeToHappen,
            Trigger = dto.Trigger
        };

        foreach (var optionDto in dto.Options)
        {
            var optionDef = new EventOptionDefinition
            {
                Text = optionDto.Text,
                Tooltip = optionDto.Tooltip,
                Effects = MiracleLoader.ConvertEffectDtos(optionDto.Effects)
            };
            eventDef.Options.Add(optionDef);
        }
        return eventDef;
    }
}