using Godot;
using System;

public partial class Bullet : RigidBody2D
{
    [Export]
    private Timer timer;

    [Export]
    private double lifeTime = 10;
    [Export]
    private Node2D attackboxComponent;
    public float speed { get; set; }

    public void SetAttackInfo(AttackComponent attackComponent) => attackboxComponent.Set("attack_component", attackComponent);


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
