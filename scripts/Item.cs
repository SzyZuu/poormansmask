using Godot;
using System;
using poormansmask.scripts;

public partial class Item : Area2D
{
	[Export] public ItemResource ItemData;

	override public void _Ready()
	{
		BodyEntered += OnItemEnter;
	}

	private void OnItemEnter(Node2D body)
	{
		GD.Print(body);
		if (body.HasMethod("ItemPickUp"))
			body.Call("ItemPickUp", ItemData);
		
		QueueFree();
	}
}
