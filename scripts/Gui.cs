using Godot;
using System;

public partial class Gui : CanvasLayer
{
	public override void _Ready()
	{
		GuiManager gm = GetNode<GuiManager>("/root/GuiManager");
		gm.Gui = this;
	}
	
	public void AddItemIcon(Texture2D sprite)
	{
		GridContainer gridContainer = GetNode<GridContainer>("%ItemGrid");
		TextureRect textureRect = new TextureRect();
		textureRect.Texture = GD.Load<Texture2D>("res://icon.svg");
		
		gridContainer.AddChild(textureRect);
	}
}
