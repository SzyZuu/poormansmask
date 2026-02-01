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
    private Abilities _ability;
    
    [Export]
    private Texture2D _itemSprite;
    
    [Export]
    private string _itemName;
    
    [Export]
    private string _itemDescription;
    
    [Export]
    private Rarities _itemRarity;
    
    public Dictionary<StatImprovements, float> GetStatImprovements() =>  _statImprovements;
    public Abilities GetAbility() =>  _ability;
    public String ItemName => _itemName;
    public String ItemDescription => _itemDescription;
    public Rarities ItemRarity => _itemRarity;
    public Texture2D ItemSprite => _itemSprite;
}