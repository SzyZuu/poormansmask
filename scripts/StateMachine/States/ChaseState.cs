using Godot;
using System;

[GlobalClass]
public partial class ChaseState : StateBase
{
	// Called when the node enters the scene tree for the first time.
	
	private CharacterBody2D _player;
	public CharacterBody2D Player => _player;

	public override void Activate()
	{
		var pm = GetNode("/root/PlayerManager") as PlayerManager;
		_player = pm.Player;
	}

	public override void Deactivate()
	{ return; }

	public override void Process(float delta)
	{ return; }

	[ExportGroup("Movement Settings")]
	[Export] private float _speed = 150f;
	[Export] private bool _flying;
	[Export] private float _flyingSpeed;
	
	[ExportGroup("Distances")]
	[Export] private Vector2 _targetDistance = new(150f, 0f);
	[Export] private float _distanceOutOfBounds = 400f;

	[ExportGroup("State References")]
	
	[Export] private StateBase _stateDistanceReached;
	[Export] private StateBase _stateOutOfBounds;

	public override void PhysicsProcess(float delta)
	{
		if (!IsInstanceValid(Player))
		{
			var pm = GetNode("/root/PlayerManager") as PlayerManager;
			_player = pm.Player;
			if (!IsInstanceValid(Player))
			{
				GD.Print("We are cooked GNG");
				return;
			}
		}

		var owner = OwnerTree.GetParent() as CharacterBody2D;

		if (!IsInstanceValid(owner))
		{
			GD.Print("FUCK");
			return;}

		int count = 0;

		if (Mathf.Abs(Player.GlobalPosition.X - owner.GlobalPosition.X) > _targetDistance.X)
		{
			count++;
			var side = Mathf.Sign(Player.GlobalPosition.X - owner.GlobalPosition.X);
			Vector2 vel = new(_speed * side, owner.Velocity.Y);
			owner.Velocity = vel;
		}

		if (Mathf.Abs(Player.GlobalPosition.Y - owner.GlobalPosition.Y) > _targetDistance.Y && _flying)
		{
			count++;
			var dir = Mathf.Sign(Player.GlobalPosition.Y - owner.GlobalPosition.Y);
			if (_flying)
			{
				Vector2 vel = new(owner.Velocity.X, dir * _speed);
				owner.Velocity = vel;
			}
		}

		if (Player.GlobalPosition.DistanceTo(owner.GlobalPosition) > _distanceOutOfBounds)
		{
			GD.Print("Out of bounds");
			OwnerTree.ChangeState(_stateOutOfBounds);
		}

		if (count < 1)
		{
			GD.Print("Reached");
			OwnerTree.ChangeState(_stateDistanceReached);
		}		
		
		if (!_flying && !owner.IsOnFloor())
		{
			owner.Velocity += (owner.GetGravity() * delta);
		}

		owner.MoveAndSlide();
	}
}
