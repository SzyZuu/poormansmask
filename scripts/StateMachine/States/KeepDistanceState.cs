using Godot;
using System;

[GlobalClass]
public partial class KeepDistanceState : StateBase
{
	private CharacterBody2D _player;
	public CharacterBody2D Player => _player;

	public override void Activate()
	{
		var pm = GetNode("/root/PlayerManager") as PlayerManager;
		_player = pm.Player;
		_timer.Start();
	}

	public override void Deactivate()
	{
		_timer.Stop();
	}

	public override void Process(float delta)
	{ return; }

	[ExportGroup("Movement Settings")]
	[Export] private float _speed = 150f;
	[Export] private bool _flying;
	[Export] private float _flyingSpeed;

	[ExportGroup("Distance Settings")]
	[Export] private float _distanceTooClose = 30;
	[Export] private float _minDistance = 100f;
	[Export] private float _maxDistance = 200f;

	[ExportGroup("Timer Settings")]
	[Export] private float _rangeTimerDuration = 3.0f;
	
	[ExportGroup("State References")]
	[Export] private StateBase _stateTooClose;
	[Export] private StateBase _stateTooFar;
	[Export] private StateBase _stateTimerExpired;

	private Timer _timer;

	public override void _Ready()
	{
		_timer = new Timer();
		_timer.OneShot = true;
		_timer.Timeout += OnTimerTimeout;
		AddChild(_timer);
	}

	private void OnTimerTimeout()
	{
		if (IsInstanceValid(Player))
		{
			var currentDistance = Player.GlobalPosition.DistanceTo((OwnerTree.GetParent() as CharacterBody2D).GlobalPosition);
			if (currentDistance >= _minDistance && currentDistance <= _maxDistance)
			{
				OwnerTree.ChangeState(_stateTimerExpired);
			}
			else
			{
				_timer.Start();
			}
		}
	}

	public override void PhysicsProcess(float delta)
	{
		if (!IsInstanceValid(Player))
		{
			var pm = GetNode("/root/PlayerManager") as PlayerManager;
			_player = pm.Player;
			if (!IsInstanceValid(Player))
			{
				return;
			}
		}

		var owner = OwnerTree.GetParent() as CharacterBody2D;

		if (!IsInstanceValid(owner))
		{
			return;
		}

		int count = 0;
		var currentDistance = Player.GlobalPosition.DistanceTo(owner.GlobalPosition);

		// Too far - move toward player
		if (currentDistance > _maxDistance)
		{
			count++;
			var direction = (Player.GlobalPosition - owner.GlobalPosition).Normalized();
			Vector2 vel = new(direction.X * _speed, owner.Velocity.Y);
			owner.Velocity = vel;
		}
		else if (currentDistance < _distanceTooClose)
		{
			if (IsInstanceValid(_stateTooClose))
			{
				Vector2 vel = new(0, owner.Velocity.Y);
				if (_flying) vel.Y = 0;
				owner.Velocity = vel;
				OwnerTree.ChangeState(_stateTooClose);
			}
		}
		// Too close - move away from player
		else if (currentDistance < _minDistance)
		{
			count++;
			var direction = (owner.GlobalPosition - Player.GlobalPosition).Normalized();
			Vector2 vel = new(direction.X * _speed, owner.Velocity.Y);
			owner.Velocity = vel;
		}

		// Stop moving if within range
		if (count < 1)
		{
			Vector2 vel = new(0, owner.Velocity.Y);
			if (_flying) vel.Y = 0;
			owner.Velocity = vel;
		}		
		
		if (!_flying && !owner.IsOnFloor())
		{
			owner.Velocity += (owner.GetGravity() * delta);
		}

		owner.MoveAndSlide();
	}
}