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
		textureRect.CustomMinimumSize = new(64, 64);
		textureRect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
		textureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspect;
		
		gridContainer.AddChild(textureRect);
	}
}
