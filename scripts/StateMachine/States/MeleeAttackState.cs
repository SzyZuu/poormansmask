using Godot;
using System;

[GlobalClass]
public partial class MeleeAttackState : StateBase
{
	[Export] private Area2D _damageArea;
	[Export] private Sprite2D _hitboxAreaVisualizer;
	[Export] private Node2D _attackRoot; // The node to flip (damageArea parent usually)
	[Export] private float _attackDuration = 1.5f;
	[Export] private int _damage = 3;
	[Export] private bool _flying = false;

	[Export] private StateBase _statePostAttack;
	
	public override void Activate()
	{
		var pm = GetNode("/root/PlayerManager") as PlayerManager;
		if (!IsInstanceValid(pm?.Player))
			return;
		
		// Flip attack area toward player
		FlipAttackTowardPlayer(pm.Player);
		
		_hitboxAreaVisualizer.Show();
		_damageArea.CollisionMask = 2;
		
		var tween = GetTree().CreateTween();
		tween.SetEase(Tween.EaseType.Out);
		tween.SetTrans(Tween.TransitionType.Quart);
		var colorFrom = new Color(1f, 1f, 1f, 0f);
		var colorTo = new Color(1f, 1f, 1f);
		_hitboxAreaVisualizer.Modulate = colorFrom;
		tween.TweenProperty(_hitboxAreaVisualizer, "modulate", colorTo, _attackDuration);
		
		tween.Finished += () =>
		{
			var pm2 = GetNode("/root/PlayerManager") as PlayerManager;
			foreach (var body in _damageArea.GetOverlappingBodies())
			{
				if (body == pm2?.Player)
				{
					pm2.Damage(_damage);
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

	private void FlipAttackTowardPlayer(CharacterBody2D player)
	{
		if (!IsInstanceValid(_attackRoot))
			_attackRoot = _damageArea.GetParent() as Node2D;
		
		var enemyPos = (OwnerTree.GetParent() as Node2D).GlobalPosition;
		var playerSide = Mathf.Sign(player.GlobalPosition.X - enemyPos.X);
		
		// Flip 180 degrees if player is on left (-1)
		if (playerSide < 0)
		{
			_attackRoot.Scale = new Vector2(-1, 1);
		}
		else
		{
			_attackRoot.Scale = new Vector2(1, 1); // Reset to normal
		}
		
		GD.Print($"Attack flipped: {playerSide < 0}"); // Debug
	}
	
	public override void Deactivate()
	{
		_hitboxAreaVisualizer.Hide();
		// Optional: Reset flip on deactivate
		if (IsInstanceValid(_attackRoot))
		{
			_attackRoot.Scale = new Vector2(1, 1);
		}
	}

	public override void Process(float delta)
	{
		var cb = OwnerTree.GetParent() as CharacterBody2D;
		if (!cb.IsOnFloor() || !_flying)
		{
			cb.Velocity += cb.GetGravity() * delta;
		}

		cb.MoveAndSlide();
	}

	public override void PhysicsProcess(float delta) {}
}