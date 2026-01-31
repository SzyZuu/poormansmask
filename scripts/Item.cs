using Godot;
using System;
using poormansmask.scripts;

public partial class Item : Area2D
{
	[Export] public ItemResource ItemData;
	Control _guiContainer;

	override public void _Ready()
	{
		BodyEntered += OnItemEnter;
		BodyExited += OnItemExit;
		
		_guiContainer = GetNode<Control>("ItemInfo");
		
		Label itemNameLabel = GetNode<Label>("ItemInfo/ItemName");
		RichTextLabel itemDescriptionLabel = GetNode<RichTextLabel>("ItemInfo/ItemDesc");
		
		itemNameLabel.Text = ItemData.ItemName;
		itemDescriptionLabel.Text = ItemData.ItemDescription;
		
	}

	private void OnItemEnter(Node2D body)
	{
		if (body.HasMethod("ItemPickUp"))
		{
			_guiContainer.Visible = true;
			//body.Call("ItemPickUp", ItemData);
			GD.Print("ItemExited");
		}
	}

	private void OnItemExit(Node2D body)
	{
		if(body.HasMethod("ItemPickUp"))
			_guiContainer.Visible = false;
	}
}
