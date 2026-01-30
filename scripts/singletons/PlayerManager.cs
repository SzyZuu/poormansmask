using Godot;
using System;

public partial class PlayerManager : Node
{
	private int _maxHealth = 10;
	private int _currentHealth;
	private int _maxArmor = 10;
	private int _currentArmor;
	private float _damageMult = 1;

	override public void _Ready()
	{
		_currentHealth = _maxHealth;
		_currentArmor = _maxArmor;
		
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
}
