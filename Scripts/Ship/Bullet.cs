using Godot;
using System;

public partial class Bullet : RigidBody2D, IDamager
{
	[Export]
	private Timer timer;

	[Export]
	private double lifeTime = 10;

	public float speed {get; set;}

	private DamageInfo damageInfo;

    public DamageInfo GetDamageInfo()
    { 
        QueueFree();
        return damageInfo;
    }

    public void SetDamageInfo(DamageInfo damageInfo) { this.damageInfo = damageInfo; }

    public override void _Ready()
    {
        timer.Start(lifeTime);
		timer.Timeout += QueueFree;
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        LinearVelocity = Transform.Y * -speed;
    }
}
