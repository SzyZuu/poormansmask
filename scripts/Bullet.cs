using Godot;

[GlobalClass]
public partial class Bullet : Area2D
{
    [Export] private float _lifetime = 5.0f;
    [Export] private int _damage = 2;
	
    public Vector2 Velocity { get; set; }
	
    public override void _Ready()
    {
        // Auto-destroy after lifetime
        GetTree().CreateTimer(_lifetime).Timeout += QueueFree;
    }
	
    public override void _PhysicsProcess(double delta)
    {
        if (!IsInstanceValid(this)) return;
		
        Position += Velocity * (float)delta;
    }
	
    public void _on_body_entered(Node2D body)
    {
        var pm = GetNode("/root/PlayerManager") as PlayerManager;
        if (body == pm?.Player)
        {
            pm.Damage(_damage);
            QueueFree();
        }
    }
}