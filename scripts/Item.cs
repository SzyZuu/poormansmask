using Godot;
using System;
using poormansmask.scripts;

public partial class Item : Area2D
{
	[Export] public ItemResource ItemData;
	Node2D _guiContainer;

	override public void _Ready()
	{
		BodyEntered += OnItemEnter;
		BodyExited += OnItemExit;
		
		_guiContainer = GetNode<Node2D>("ItemInfo");
		PanelContainer panelContainer = GetNode<PanelContainer>("ItemInfo/PanelContainer");
		
		Label itemNameLabel = GetNode<Label>("%ItemName");
		RichTextLabel itemDescriptionLabel = GetNode<RichTextLabel>("%ItemDesc");
		
		itemNameLabel.Text = ItemData.ItemName;
		itemDescriptionLabel.Text = ItemData.ItemDescription;
		
		//panelContainer.SetSize(GetNode<MarginContainer>("ItemInfo/PanelContainer/MarginContainer").GetGlobalRect().Size, true);
		//panelContainer.OffsetBottom = 4;		// idk what the fuck is happening
	}

	private void OnItemEnter(Node2D body)
	{
		if (body.HasMethod("ItemPickUp"))
		{
			_guiContainer.Visible = true;
			//body.Call("ItemPickUp", ItemData);
		}
	}

	private void OnItemExit(Node2D body)
	{
		if(body.HasMethod("ItemPickUp"))
			_guiContainer.Visible = false;
	}
}
