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
        GetTree().CreateTimer(_lifetime).Timeout += Destroy;
        
        BodyEntered += OnBodyEntered;
    }
	
    public override void _PhysicsProcess(double delta)
    {
        if (!IsInstanceValid(this)) return;
		
        Position += Velocity * (float)delta;
    }
	
    // Renamed to match signal convention + fixed logic
    private void OnBodyEntered(Node2D body)
    {
        GD.Print($"Bullet hit: {body.Name}"); // Debug
        
        var pm = GetNode("/root/PlayerManager") as PlayerManager;
        if (body == pm?.Player)
        {
            GD.Print($"Damaging player for {_damage}"); // Debug
            pm.Damage(_damage);
        }

        Destroy();
    }

    void Destroy()
    {
        var pc = GetNode("%ParticlesCollision") as CpuParticles2D;
        if (pc != null)
        {
            pc.Reparent(GetParent());
            pc.Restart();
            pc.Finished += pc.QueueFree;
        }
        QueueFree();
    }
}