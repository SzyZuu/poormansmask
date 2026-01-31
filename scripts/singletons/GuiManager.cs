using Godot;
using System;

public partial class GuiManager : Node
{
	public Gui Gui;

	public void ItemAdded(Texture2D item)
	{
		Gui.AddItemIcon(item);
	}
}
