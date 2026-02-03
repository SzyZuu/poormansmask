using Godot;
using System;
using poormansmask.scripts.singletons;

public partial class Exit : Area2D
{
	public override void _Ready()
	{
		LevelManager lm = GetNode<LevelManager>("/root/LevelManager");
		lm.StageCleared += OnStageCleared;
	}

	private void OnStageCleared()
	{
		Tween tween = GetTree().CreateTween();
		tween.SetEase(Tween.EaseType.In);
		tween.SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(GetNode<Sprite2D>("Sprite2D"), "modulate", Colors.White, 1.5f);
	}
}
