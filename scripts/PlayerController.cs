using Godot;
using System;
using System.Collections.Generic;
using poormansmask.scripts;
using poormansmask.scripts.enums;
using poormansmask.scripts.interfaces;

public partial class PlayerController : CharacterBody2D, IPickUp
{
	[Export] private float _jumpHeight = 100f;
	[Export] private float _jumpTimeToPeak = 0.4f;
	[Export] private float _jumpTimeToDescent = 0.4f;

	private float _jumpVelocity;
	private float _jumpGravity;
	private float _fallGravity;
	public const float Speed = 300.0f;
	private float _attackCooldown = 0.1f;
	private int _baseDamage = 10;
	private int _jumps = 1;
	private int _jumpsLeft = 0;
	private bool _coyoteActive;
	private bool _lastFrameFloor;
	private bool _canAttack = true;
	
	
	private List<IAbility> _activeAbilities = new();
	
	private IAmItem _itemInRange;
	private IInventory _inventory;

	private Area2D _meleeRange;

	public override void _Ready()
	{
		PlayerManager pm =  GetNode<PlayerManager>("/root/PlayerManager");
		_inventory = pm;
		pm.Player = this;
		
		_meleeRange = GetNode<Area2D>("MeleeRange");

		_jumpVelocity = (2 * _jumpHeight / _jumpTimeToPeak) * -1;
		_jumpGravity = (2 * _jumpHeight) / (_jumpTimeToPeak * _jumpTimeToPeak);
		_fallGravity = (2 * _jumpHeight) / (_jumpTimeToDescent * _jumpTimeToDescent);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			if (!_coyoteActive && _lastFrameFloor)
			{
				_coyoteActive = true;
				GetTree().CreateTimer(0.2).Timeout += () =>
				{
					_coyoteActive = false;
				};
			}
			if(velocity.Y < 0)
				velocity.Y += _jumpGravity * (float)delta;
			else
				velocity.Y += _fallGravity * (float)delta;
			_lastFrameFloor = false;
		}else
		{
			_lastFrameFloor = true;
			_jumpsLeft = _jumps;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && (IsOnFloor() || _coyoteActive || _jumpsLeft > 0))
		{
			velocity.Y = _jumpVelocity;
			_jumpsLeft--;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		float direction = Input.GetAxis("left", "right");
		if (direction != 0f)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, direction * Speed, Speed * (float)delta * 8f);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed * (float)delta * 8f);
		}

		Velocity = velocity;
		MoveAndSlide();
		
		foreach(IAbility ability in _activeAbilities)
		{
			ability.Passive(this);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Interact") && _itemInRange != null)
			ItemPickUp(_itemInRange);

		if (@event.IsActionPressed("Attack") && _canAttack)
			Attack();
		
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			UpdateMeleeDirection(eventMouseMotion.Position);
		}

		foreach (IAbility ability in _activeAbilities)
		{
			ability.ActiveAction(this, @event);
		}
	}

	private void UpdateMeleeDirection(Vector2 mousePosition)
	{
		float screenCenter = GetViewportRect().Size.X / 2;
		bool isLeft = mousePosition.X < screenCenter;

		float newXScale = Math.Abs(_meleeRange.Scale.X) * (isLeft ? -1 : 1);
		if(!Mathf.IsEqualApprox(_meleeRange.Scale.X, newXScale))
			_meleeRange.SetScale(new(newXScale, _meleeRange.Scale.Y));
	}

	public void ItemPickUp(IAmItem item)
	{
		_inventory.AddItem(item.GetItemResource());
		item.GotPickedUp();
	}

	public void ItemInRange(IAmItem item)
	{
		_itemInRange = item;
	}

	public void ItemOutOfRange()
	{
		_itemInRange = null;
	}

	public void AbilityAdded(IAbility ability)
	{
		_activeAbilities.Add(ability);
	}

	public void AddJumps(int jumpAmount)
	{
		_jumps += jumpAmount;
	}

	private void Attack()
	{
		GD.Print("HIYA");
		PlayerManager pm = GetNode<PlayerManager>("/root/PlayerManager");
		_canAttack = false;

		GetTree().CreateTimer(_attackCooldown).Timeout += () =>
		{
			_canAttack = true;
		};

		foreach (var body in _meleeRange.GetOverlappingBodies())
		{
			if(body is IDamageable enemy)
				enemy.Damage((int)(_baseDamage * pm.GetStat(StatImprovements.DAMAGEMULTIPLIER)));
		}
	}
}
