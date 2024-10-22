using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
    const float SPEED_FACTOR = 10;

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

    public override void _PhysicsProcess(double delta)
    {
        MoveAndCollide(-Transform.Y * speed * (float)delta * SPEED_FACTOR);
    }

    public void OnAttackboxAreaEntered(Area2D area)
    {

        QueueFree();
    }
}
