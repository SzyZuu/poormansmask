using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class WanderingState : StateBase
{
    public override void Activate()
    {
    }

    public override void Deactivate()
    {
    }

    public override void Process(float delta)
    {
    }

    private int _dir = 1;

    [Export]
    private StateBase _stateOnPlayerDetect;
    
    [Export]
    private float _speed = 150f;
    int frameCounter = 0;
    public override void PhysicsProcess(float delta)
    {
        frameCounter++; 
        var parent = OwnerTree.Owner as CharacterBody2D;

        UpdateDir();
        if (frameCounter >= 5)
        {
            frameCounter = 0;
            LookForPlayer();
        }

        var parentVelY = parent.Velocity.Y;
        
        if (!parent.IsOnFloor())
        {
            parentVelY += (parent.GetGravity() * delta).Y;
        }
        parent.Velocity = new(_dir * _speed, parentVelY);
        parent.MoveAndSlide();
    }

    void UpdateDir()
    {
        var parent = OwnerTree.Owner as CharacterBody2D;
        var raySidePos = _dir * (_speed * .25f) + parent.GlobalPosition.X;
        var querySide = PhysicsRayQueryParameters2D.Create(parent.GlobalPosition,
            new Vector2(raySidePos, parent.GlobalPosition.Y), 1);
        var dirSpaceState = GetTree().GetRoot().World2D.GetDirectSpaceState();
        var resultSide = dirSpaceState.IntersectRay(querySide);
        if (resultSide.ContainsKey("position"))
        {
            _dir *= -1;
        }
        else
        {
            var querySideDown = PhysicsRayQueryParameters2D.Create(
                new Vector2(raySidePos, parent.GlobalPosition.Y - 10),
                new Vector2(raySidePos, parent.GlobalPosition.Y + 50));
            var resultSideDown = dirSpaceState.IntersectRay(querySideDown);
            if (!resultSideDown.ContainsKey("position"))
            {
                _dir *= -1;
            }
        }
    }

    [Export]
    private int _viewResolution = 5;
    void LookForPlayer()
    {
        var parent = OwnerTree.Owner as CharacterBody2D;

        var dirSpaceState = GetTree().GetRoot().World2D.GetDirectSpaceState();
        var dir = new Vector2(_dir, 0);
        var querySide = PhysicsRayQueryParameters2D.Create(parent.GlobalPosition, parent.GlobalPosition + dir * (_speed *   3f), 2);
        var resultSide = dirSpaceState.IntersectRay(querySide);
        float maxRotRad = (float)Math.PI/2f;
        float rotStep = maxRotRad / _viewResolution;
        if (resultSide.ContainsKey("position"))
        {
            OwnerTree.ChangeState(_stateOnPlayerDetect);
            return;
        }
        for (int i = 0; i < _viewResolution; i++)
        {
            for (int x = -1; x <= 1; x += 2)
            {
                float rot = rotStep * x * i;
                var rayDir = dir.Rotated(rot);
                var query = PhysicsRayQueryParameters2D.Create(parent.GlobalPosition, parent.GlobalPosition + rayDir * (_speed * 3f), 2);
                var res = dirSpaceState.IntersectRay(query);
                if (res.ContainsKey("position"))
                {
                    OwnerTree.ChangeState(_stateOnPlayerDetect);
                    return;
                }
            }
        }
    }
}



