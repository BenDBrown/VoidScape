using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
    const float SPEED_FACTOR = 10;

	[Signal]
	public delegate void HitEventHandler();

    [Export]
    private Timer timer;

    [Export]
    private double lifeTime = 10;
    [Export]
    private Node2D attackboxComponent;
    [Export]
    private Node2D bulletSprite;
    [Export]
    private SingleRunAnimation bulletHitAnimation;

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
        if (area.GetParent() == this) { return; }

        if(area.GetParent().GetParent() is Ship){ //Todo: Better definition for enemies should be made
            EmitSignal(SignalName.Hit);
        }

        bulletSprite.Visible = false;
        bulletHitAnimation.AnimationFinished += DestroyBullet;
        bulletHitAnimation.Play();
    }

    public void OnAttackBoxBodyEntered(Node2D node2D)
    {
        if (node2D == this) { return; }
        bulletSprite.Visible = false;
        bulletHitAnimation.AnimationFinished += DestroyBullet;
        DestroyBullet();
    }

    private void DestroyBullet(){
        CallDeferred("queue_free");
    }
}
