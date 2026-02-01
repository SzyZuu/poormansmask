using Godot;
using System;
using poormansmask.scripts;
using poormansmask.scripts.enums;
using poormansmask.scripts.interfaces;

public partial class Item : Area2D, IAmItem
{
	[Export] public ItemResource ItemData;
	Node2D _guiContainer;

	override public void _Ready()
	{
		BodyEntered += OnItemEnter;
		BodyExited += OnItemExit;
		
		_guiContainer = GetNode<Node2D>("ItemInfo");
		
		Label itemNameLabel = GetNode<Label>("%ItemName");
		RichTextLabel itemDescriptionLabel = GetNode<RichTextLabel>("%ItemDesc");
		
		itemNameLabel.Text = ItemData.ItemName;
		itemDescriptionLabel.Text = ItemData.ItemDescription + "\n";
		
		AddColoredStats(itemDescriptionLabel);
		itemDescriptionLabel.AddText("\n");
		AddAbilityDesc(itemDescriptionLabel);
	}

	private void AddColoredStats(RichTextLabel label)
	{
		foreach (var stat in ItemData.GetStatImprovements())
		{
			if (stat.Value < 0)
			{
				label.PushColor(new Color("#F54D54"));

				if (stat.Key == StatImprovements.DAMAGEMULTIPLIER)
				{
					label.AddText("\n-" + (int)(stat.Value * 100) + StatNameHelper.GetDisplayString(stat.Key));
				}else
					label.AddText("\n" + stat.Value + " " + StatNameHelper.GetDisplayString(stat.Key));
			}else if (stat.Value > 0)
			{
				label.PushColor(new Color("#84EB3B"));

				if (stat.Key == StatImprovements.DAMAGEMULTIPLIER)
				{
					label.AddText("\n+" + (int)(stat.Value * 100) + StatNameHelper.GetDisplayString(stat.Key));
				}else
					label.AddText("\n+" + stat.Value + " " + StatNameHelper.GetDisplayString(stat.Key));
			}
		}
	}

	private void AddAbilityDesc(RichTextLabel label)
	{
		label.PushColor(new Color("#F5F0B3"));
		label.AddText("\n" + AbilityDescHelper.GetAbilityDesc(ItemData.GetAbility()));
	}

	private void OnItemEnter(Node2D body)
	{
		if (body is IPickUp pickup)
		{
			_guiContainer.Visible = true;
			pickup.ItemInRange(this);
		}
	}

	private void OnItemExit(Node2D body)
	{
		if(body is IPickUp pickup)
		{
			_guiContainer.Visible = false;
			pickup.ItemOutOfRange();
		}
	}

	public ItemResource GetItemResource()
	{
		return ItemData;
	}

	public void GotPickedUp()
	{
		GuiManager gui = GetNode<GuiManager>("/root/GuiManager");
		PlayerManager pm = GetNode<PlayerManager>("/root/PlayerManager");
		
		gui.ItemAdded(ItemData.ItemSprite);

		Abilities ability = ItemData.GetAbility();
		if (ability != Abilities.NONE)
			pm.ActivateAbility(ability);
		
		QueueFree();
	}
}
