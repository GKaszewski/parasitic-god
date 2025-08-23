using System;

namespace ParasiticGod.Scripts.Core;

public class StatData
{
    public double Value { get; private set; }
    public event Action<double> OnChanged;

    public void Set(double value)
    {
        if (!(Math.Abs(Value - value) > 0.001)) return;
        
        Value = value;
        OnChanged?.Invoke(Value);
    }
}