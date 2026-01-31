using System;
using Godot;
using Godot.Collections;
using poormansmask.scripts.enums;

namespace poormansmask.scripts;

[GlobalClass]
public partial class ItemResource : Resource
{
    [Export]
    private Dictionary<StatImprovements, float> _statImprovements = new Dictionary<StatImprovements, float>();
    
    [Export]
    private Texture2D _itemSprite;
    
    [Export]
    private string _itemName;
    
    [Export]
    private string _itemDescription;
    
    [Export]
    private Rarities _itemRarity;
    
    public Dictionary<StatImprovements, float> GetStatImprovements() =>  _statImprovements;
}