using Godot;
using System;

public partial class GuiManager : Node
{
	public Gui Gui;

	public void ItemAdded(Texture2D item)
	{
		Gui.AddItemIcon(item, "%ItemGrid");
	}

	public void AbilityAdded(Texture2D item)
	{
		Gui.AddItemIcon(item, "%AbilityGrid");
	}
}
