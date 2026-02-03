using Godot;
using System;
using poormansmask.scripts.interfaces;
using poormansmask.scripts.singletons;

public partial class Enemy : CharacterBody2D, IDamageable
{
	[Export] public int Health = 50;

	[Signal]
	public delegate void DeadEventHandler();

	public override void _Ready()
	{
		LevelManager lm = GetNode<LevelManager>("/root/LevelManager");
		lm.RegisterEnemy(this);
	}

	public void Damage(int amount)
	{
		GD.Print("Ouch");
		Health -= amount;
		
		if(Health <= 0)
		{
			EmitSignal(SignalName.Dead);
			QueueFree();
		}
	}
}
