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
    [Export]
    private Node2D bulletSprite;
    [ExportCategory("On-Hit Configuration")]
    [Export]
    private SingleRunAnimation bulletHitAnimation;
    [Export]
	private SingleRunAudio onHitAudio;

    public float speed { get; set; }

    public void SetAttackInfo(AttackComponent attackComponent) => attackboxComponent.Set("attack_component", attackComponent);


    public override void _Ready()
    {
        timer.Start(lifeTime);
        timer.Timeout += QueueFree;
        attackboxComponent.Connect("on_hit", Callable.From(OnHitFX));
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndCollide(-Transform.Y * speed * (float)delta * SPEED_FACTOR);
    }

    public void OnAttackboxAreaEntered(Area2D area)
    {
        if (area.GetParent() == this) { return; }
    }

    public void OnAttackBoxBodyEntered(Node2D node2D)
    {
        if (node2D == this) { return; }
    }

    private void DestroyBullet(){
        CallDeferred("queue_free");
    }

    /// <summary>
    /// Visual and Sound effects when a bullet has hit an HitboxComponent
    /// </summary>
    private void OnHitFX(){
        // SFX
        onHitAudio.PlayOnce();

        // VFX
        bulletSprite.Visible = false;
        bulletHitAnimation.AnimationFinished += DestroyBullet;
        bulletHitAnimation.Visible = true;
        bulletHitAnimation.Play();
    }
}
