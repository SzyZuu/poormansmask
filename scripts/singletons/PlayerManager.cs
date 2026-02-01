using Godot;
using System;
using System.Collections.Generic;
using poormansmask.scripts;
using poormansmask.scripts.enums;
using poormansmask.scripts.interfaces;
using poormansmask.scripts.repositories;

public partial class PlayerManager : Node, IInventory
{
	private int _currentArmor;
	private int _currentHealth;

	Godot.Collections.Dictionary<StatImprovements, float> _stats = new()
	{
		[StatImprovements.MAXHEALTH] = 100,
		[StatImprovements.MAXARMOR] = 10,
		[StatImprovements.DAMAGEMULTIPLIER] = 1,
	};
	
	private List<ItemResource> _inventory = new List<ItemResource>();
	private AbilityRepository _abilityRepository = new AbilityRepository();

	private CharacterBody2D _playerBody;
	//private CharacterBody2D _body2D;

	[Export]
	public CharacterBody2D Player
	{
		get => _playerBody;
		set
		{
			_playerBody = value;
		}
	}
	
	override public void _Ready()
	{
		_currentHealth = (int)_stats[StatImprovements.MAXHEALTH];
		_currentArmor = (int)_stats[StatImprovements.MAXARMOR];
		
		var timer = new Timer();
		AddChild(timer);
		timer.OneShot = false;
		timer.Start(1);
		timer.Timeout += () =>
		{
			_currentArmor += 1;
		};
	}

	public int Damage(int amount)
	{ 
		int dealtArmor = Math.Min(amount, _currentArmor);
		_currentArmor -= dealtArmor;
		int dealtHealth = Math.Max(amount - dealtArmor, 0);
		_currentHealth -= dealtHealth;
		return dealtArmor + dealtHealth;
	}

	public void AddItem(ItemResource itemResource)
	{
		_inventory.Add(itemResource);

		var statImprovements = itemResource.GetStatImprovements();
		foreach (var statImprovement in statImprovements)
		{
			_stats[statImprovement.Key] += statImprovement.Value;
		}
	}

	public void ActivateAbility(Abilities abilityToActivate)
	{
		IAbility ability = _abilityRepository.GetAbility(abilityToActivate);

		PlayerController pc = _playerBody as PlayerController;
		ability.Activate(pc);
		pc.AbilityAdded(ability);
	}
}
