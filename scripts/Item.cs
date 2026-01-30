using System;
using poormansmask.scripts.enums;

namespace poormansmask.scripts;

public abstract class Item
{
    private float[] _statImprovements = new float[Enum.GetValues(typeof(StatImprovements)).Length];
    
    public float[] GetStatImprovements() =>  _statImprovements;
}