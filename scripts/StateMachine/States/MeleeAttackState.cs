using Godot;
using System;

[GlobalClass]
public partial class MeleeAttackState : StateBase
{
	[Export] private Area2D _damageArea;
	[Export] private Sprite2D _hitboxAreaVisualizer;
	[Export] private float _attackDuration = 1.5f;
	[Export] private int _damage = 2;

	[Export] private StateBase _statePostAttack;
	
	public override void Activate()
	{
		_hitboxAreaVisualizer.Show();
		_damageArea.CollisionMask = 2;
		var tween = GetTree().CreateTween();
		tween.SetEase(Tween.EaseType.In);
		tween.SetTrans(Tween.TransitionType.Quart);
		var colorFrom = new Color(1f, .1f, .1f, 0f);
		var colorTo = new Color(1f, .1f, .1f);
		_hitboxAreaVisualizer.Modulate = colorFrom;
		tween.TweenProperty(_hitboxAreaVisualizer, "modulate", colorTo, _attackDuration);
		tween.Finished += () =>
		{
			var pm = GetNode("/root/PlayerManager") as PlayerManager;
			foreach (var body in _damageArea.GetOverlappingBodies())
			{
				if (body == pm.Player)
				{
					pm.Damage(_damage);
				}
			}
			if (IsInstanceValid(_statePostAttack))
			{
				OwnerTree.ChangeState(_statePostAttack);
			}
			else
			{
				OwnerTree.ChangeState(GetParent().GetParent() as StateBase);
			}
		};
	}

	public override void Deactivate()
	{
		_hitboxAreaVisualizer.Hide();
	}

	public override void Process(float delta) {}

	public override void PhysicsProcess(float delta) {}
}
