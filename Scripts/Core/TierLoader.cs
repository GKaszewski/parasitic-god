using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json;

namespace ParasiticGod.Scripts.Core;

public static class TierLoader
{
    public static List<TierDefinition> LoadTiersFromFile(string filePath)
    {
        var loadedTiers = new List<TierDefinition>();
        
        var fileContent = FileAccess.GetFileAsString(filePath);
        if (string.IsNullOrEmpty(fileContent))
        {
            GD.PushError($"Failed to read tier file or file is empty: {filePath}");
            return loadedTiers;
        }

        var tierListDto = JsonConvert.DeserializeObject<TierListDto>(fileContent);
        if (tierListDto?.Tiers == null)
        {
            GD.PushError($"Failed to deserialize tier list JSON or 'Tiers' array is missing: {filePath}");
            return loadedTiers;
        }

        foreach (var dto in tierListDto.Tiers)
        {
            var tierDef = new TierDefinition
            {
                Threshold = dto.Threshold,
                TierEnum = dto.TierEnum,
                Scene = GD.Load<PackedScene>(dto.ScenePath) 
            };
            loadedTiers.Add(tierDef);
        }

        GD.Print($"Loaded {loadedTiers.Count} follower tiers from {filePath}");
        return loadedTiers.OrderBy(t => t.Threshold).ToList();
    }
}