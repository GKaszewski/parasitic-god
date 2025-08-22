using System;
using System.Text.Json;
using Godot;
using Godot.Collections;
using Newtonsoft.Json;
using ParasiticGod.Scripts.Core.Effects;

namespace ParasiticGod.Scripts.Core;

public static class MiracleLoader
{
    private static readonly System.Collections.Generic.Dictionary<string, Type> EffectRegistry = new()
    {
        { "AddResource", typeof(AddResourceEffect) },
        { "ApplyBuff", typeof(ApplyBuffEffect) },
        { "ConvertResource", typeof(ConvertResourceEffect) },
        { "ModifyStat", typeof(ModifyStatEffect) },
        { "UnlockMiracle", typeof(UnlockMiracleEffect) },
        { "DestroySelf", typeof(DestroySelfEffect) }
    };

    public static System.Collections.Generic.Dictionary<string, MiracleDefinition> LoadMiraclesFromDirectory(string path)
    {
        var loadedMiracles = new System.Collections.Generic.Dictionary<string, MiracleDefinition>();
        using var dir = DirAccess.Open(path);
        if (dir == null) return loadedMiracles;

        dir.ListDirBegin();
        var fileName = dir.GetNext();
        while (!string.IsNullOrEmpty(fileName))
        {
            if (!dir.CurrentIsDir() && fileName.EndsWith(".json"))
            {
                var filePath = path.PathJoin(fileName);
                var fileNameNoExt = fileName.GetBaseName();
                var miracle = LoadMiracleFromFile(filePath, fileNameNoExt); // Pass the ID
                if (miracle != null)
                {
                    loadedMiracles.Add(fileNameNoExt, miracle);
                }
            }
            fileName = dir.GetNext();
        }
        
        GD.Print($"Loaded {loadedMiracles.Count} miracles from {path}");
        return loadedMiracles;
    }

    private static MiracleDefinition LoadMiracleFromFile(string filePath, string miracleId)
    {
        var fileContent = FileAccess.GetFileAsString(filePath);
        if (string.IsNullOrEmpty(fileContent))
        {
            GD.PushError($"Failed to read file or file is empty: {filePath}");
            return null;
        }

        var miracleDto = JsonConvert.DeserializeObject<MiracleDto>(fileContent);
        if (miracleDto == null)
        {
            GD.PushError($"Failed to deserialize miracle JSON: {filePath}");
            return null;
        }

        var miracleDef = new MiracleDefinition
        {
            Id = miracleId,
            UnlockedByDefault = miracleDto.UnlockedByDefault,
            Name = miracleDto.Name,
            FaithCost = miracleDto.FaithCost,
            FollowersRequired = miracleDto.FollowersRequired,
            Effects = []
        };

        foreach (var effectDto in miracleDto.Effects)
        {
            if (EffectRegistry.TryGetValue(effectDto.Type, out var effectType))
            {
                var effectInstance = (Effect)Activator.CreateInstance(effectType);
                switch (effectInstance)
                {
                    case AddResourceEffect addResourceEffect:
                        addResourceEffect.TargetResource = effectDto.TargetResource;
                        addResourceEffect.Value = effectDto.Value;
                        break;
                    case ApplyBuffEffect applyBuffEffect:
                        applyBuffEffect.TargetStat = effectDto.TargetBuffStat;
                        applyBuffEffect.Multiplier = effectDto.Multiplier;
                        applyBuffEffect.Duration = effectDto.Duration;
                        break;
                    case ConvertResourceEffect convertResourceEffect:
                        convertResourceEffect.FromResource = effectDto.FromResource;
                        convertResourceEffect.FromAmount = effectDto.FromAmount;
                        convertResourceEffect.ToResource = effectDto.ToResource;
                        convertResourceEffect.ToAmount = effectDto.ToAmount;
                        break;
                    case ModifyStatEffect modifyStatEffect:
                        modifyStatEffect.TargetStat = effectDto.TargetStat;
                        modifyStatEffect.Op = effectDto.Op;
                        modifyStatEffect.Value = effectDto.Value;
                        break;
                    case UnlockMiracleEffect unlockMiracleEffect:
                        unlockMiracleEffect.MiraclesToUnlock = new Array<string>(effectDto.MiraclesToUnlock);
                        break;
                }

                miracleDef.Effects.Add(effectInstance);
            }
        }

        return miracleDef;
    }
}