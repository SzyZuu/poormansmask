using Godot;
using poormansmask.scripts.singletons;

public partial class Exit : Area2D
{
	[Signal]
	public delegate void PortalEnteredEventHandler();
	
	private bool _finished;
	
	public override void _Ready()
	{
		LevelManager lm = GetNode<LevelManager>("/root/LevelManager");
		lm.StageCleared += OnStageCleared;
		BodyEntered += OnBodyEntered;
		
		lm.SetCurrentExit(this);
	}

	private void OnBodyEntered(Node2D body)
	{
		if (_finished)
			EmitSignal(SignalName.PortalEntered);
	}

	private void OnStageCleared()
	{
		Tween tween = GetTree().CreateTween();
		tween.SetEase(Tween.EaseType.In);
		tween.SetTrans(Tween.TransitionType.Quad);
		tween.TweenProperty(GetNode<Sprite2D>("Sprite2D"), "modulate", Colors.White, 1.5f);

		_finished = true;
	}
}
