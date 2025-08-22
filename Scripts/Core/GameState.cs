using System.Collections.Generic;
using ParasiticGod.Scripts.Core.Effects;

namespace ParasiticGod.Scripts.Core;

public class GameState
{
    public double Faith { get; set; } = 50.0;
    public double FaithPerFollower { get; set; } = 0.5;
    public long Followers { get; set; } = 40;
    public double Corruption { get; set; } = 0.0;

    public List<Buff> ActiveBuffs { get; } = [];

    public double FaithPerSecond
    {
        get
        {
            var totalMultiplier = 1.0;
            foreach (var buff in ActiveBuffs)
            {
                totalMultiplier *= buff.Multiplier;
            }
            return Followers * FaithPerFollower * totalMultiplier;
        }
    }
}