using Godot;
using System;
using poormansmask.scripts;
using poormansmask.scripts.interfaces;

public partial class PlayerController : CharacterBody2D, IPickUp
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;
	private bool _coyoteActive = false;
	private bool lastFrameFloor = false;
	
	private IAmItem _itemInRange;
	private IInventory _inventory;

	public override void _Ready()
	{
		PlayerManager pm =  GetNode<PlayerManager>("/root/PlayerManager");
		_inventory = pm;
		pm.Player = this;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			if (!_coyoteActive && lastFrameFloor)
			{
				_coyoteActive = true;
				GetTree().CreateTimer(0.2).Timeout += () =>
				{
					_coyoteActive = false;
				};
			}
			velocity += GetGravity() * (float)delta;
			lastFrameFloor = false;
		}else
			lastFrameFloor = true;

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && (IsOnFloor() || _coyoteActive))
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		float direction = Input.GetAxis("left", "right");
		if (direction != 0f)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, direction * Speed, Speed * (float)delta * 4f);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed * (float)delta * 4f);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Interact") && _itemInRange != null)
		{
			ItemPickUp(_itemInRange);
		}
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
}
