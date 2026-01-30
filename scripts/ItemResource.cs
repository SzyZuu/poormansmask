using System;
using Godot;
using poormansmask.scripts.enums;

namespace poormansmask.scripts;

[GlobalClass]
public partial class ItemResource : Resource
{
    [Export]
    private float[] _statImprovements = new float[Enum.GetValues(typeof(StatImprovements)).Length];
    
    [Export]
    private Texture2D _itemSprite;
    
    [Export]
    private string _itemName;
    
    [Export]
    private string _itemDescription;
    
    [Export]
    private Rarities _itemRarity;
    
    public float[] GetStatImprovements() =>  _statImprovements;
}