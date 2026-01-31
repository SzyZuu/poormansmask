using Godot;
using System;
using System.Collections.Generic;
using poormansmask.scripts;
using poormansmask.scripts.enums;
using poormansmask.scripts.interfaces;

public partial class PlayerManager : Node, IPickUp
{
	private int _currentArmor;
	private int _currentHealth;
	
	float[] _stats = new float[Enum.GetValues(typeof(StatImprovements)).Length];
	private List<ItemResource> _inventory = new List<ItemResource>();

	private CharacterBody2D _playerBody;

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
		for (int i = 0; i < _stats.Length; i++)
		{
			_stats[i] = (StatImprovements)i switch
			{
				StatImprovements.DAMAGEMULTIPLIER => 1,
				StatImprovements.MAXARMOR => 10,
				StatImprovements.MAXHEALTH => 10,
			};
		}
		
		_currentHealth = (int)_stats[(int)StatImprovements.MAXHEALTH];
		_currentArmor = (int)_stats[(int)StatImprovements.MAXARMOR];
		
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

	public void ItemPickUp(ItemResource itemResource)
	{
		_inventory.Add(itemResource);
		
		for (int i = 0; i < Enum.GetValues(typeof(StatImprovements)).Length; i++)
		{
			_stats[i] += itemResource.GetStatImprovements()[i];
		}
	}
}
