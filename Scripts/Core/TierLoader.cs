using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json;

namespace ParasiticGod.Scripts.Core;

public static class TierLoader
{
    public static List<TierDefinition> LoadTiers(string baseFilePath, string userFilePath)
    {
        // Prioritize the user's file. If it exists, load it.
        if (FileAccess.FileExists(userFilePath))
        {
            GD.Print($"Loading user tier file: {userFilePath}");
            return LoadTierListFromFile(userFilePath);
        }
        
        // Otherwise, fall back to the base game's file.
        GD.Print($"Loading base tier file: {baseFilePath}");
        return LoadTierListFromFile(baseFilePath);
    }
    
    private static List<TierDefinition> LoadTierListFromFile(string filePath)
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
            Texture2D texture = null;
            if (dto.ImagePath.StartsWith("res://"))
            {
                texture = GD.Load<Texture2D>(dto.ImagePath);
            }
            else if (dto.ImagePath.StartsWith("user://"))
            {
                var image = Image.LoadFromFile(dto.ImagePath);
                if (image != null)
                {
                    texture = ImageTexture.CreateFromImage(image);
                }
            }
            
            var tierDef = new TierDefinition
            {
                Threshold = dto.Threshold,
                TierEnum = dto.TierEnum,
                Texture = texture,
                Scale = new Vector2(dto.Scale.X, dto.Scale.Y)
            };
            loadedTiers.Add(tierDef);
        }

        GD.Print($"Loaded {loadedTiers.Count} follower tiers from {filePath}");
        return loadedTiers.OrderBy(t => t.Threshold).ToList();
    }
}