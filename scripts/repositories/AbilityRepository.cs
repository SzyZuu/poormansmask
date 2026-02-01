using System.Collections.Generic;
using Godot;
using poormansmask.scripts.abilities;
using poormansmask.scripts.enums;
using poormansmask.scripts.interfaces;

namespace poormansmask.scripts.repositories;

public class AbilityRepository
{
    private Dictionary<Abilities, IAbility> _abilityDictionary;

    public AbilityRepository()
    {
        _abilityDictionary = new()
        {
            [Abilities.JUMPADD] = new DoubleJump(),
            [Abilities.DOUBLESHOT] = new DoubleShot(),
            [Abilities.TELEPORT] = new Galatea()
        };
    }

    public IAbility GetAbility(Abilities ability)
    {
        if(_abilityDictionary.TryGetValue(ability, out var abilityClass))
            return abilityClass;
        
        GD.Print("Ability not registered");
        return null;
    }
}