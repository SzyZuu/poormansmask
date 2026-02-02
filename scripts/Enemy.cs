using Godot;
using System;
using poormansmask.scripts.interfaces;

public partial class Enemy : CharacterBody2D, IDamageable
{
	[Export] public int Health = 50;

	[Signal]
	public delegate void DeadEventHandler();

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
