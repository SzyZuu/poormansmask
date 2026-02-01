using Godot;
using System;

[GlobalClass]
public partial class ShootBulletState : StateBase
{
    [Export] private PackedScene _bulletScene;
    [Export] private Node2D _bulletSpawnPoint;
    [Export] private float _bulletSpeed = 300f;
	
    [Export] private StateBase _statePostShoot;
	
    public override void Activate()
    {
        var pm = GetNode("/root/PlayerManager") as PlayerManager;
        if (!IsInstanceValid(pm?.Player) || !IsInstanceValid(_bulletSpawnPoint))
            return;

        // Calculate direction to player
        var direction = (pm.Player.GlobalPosition - _bulletSpawnPoint.GlobalPosition).Normalized();
		
        // Instance bullet
        var bullet = _bulletScene.Instantiate<Bullet>();
        GetTree().CurrentScene.AddChild(bullet);
		
        // Position bullet at spawn point
        bullet.GlobalPosition = _bulletSpawnPoint.GlobalPosition;
		
        // Set bullet velocity toward player
        bullet.Velocity = direction * _bulletSpeed;
		
        // Immediately transition to next state
        if (IsInstanceValid(_statePostShoot))
        {
            OwnerTree.ChangeState(_statePostShoot);
        }
        else
        {
            OwnerTree.ChangeState(GetParent().GetParent() as StateBase);
        }
    }

    public override void Deactivate() {}

    public override void Process(float delta) {}

    public override void PhysicsProcess(float delta) {}
}