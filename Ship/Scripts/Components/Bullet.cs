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
	private SingleRunAudio onHitAudio;
    [Export]
    private PackedScene bulletHitAnimationPrefab;

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

        OnHitFX();
    }

    public void OnAttackBoxBodyEntered(Node2D node2D)
    {
        if (node2D == this) { return; }

        OnHitFX();
    }

    private void DestroyBullet(){

        // Bullet Death VFX
        AnimatedSprite2D bulletDeathAnimation = bulletHitAnimationPrefab.Instantiate() as AnimatedSprite2D;
        bulletDeathAnimation.GlobalPosition = GlobalPosition;
        bulletDeathAnimation.GlobalRotation = GlobalRotation;
        bulletDeathAnimation.Play();
        GetTree().CurrentScene.AddChild(bulletDeathAnimation);

        // Destroy Bullet
        CallDeferred("queue_free");
    }

    /// <summary>
    /// Visual and Sound effects when a bullet has hit an HitboxComponent
    /// </summary>
    private void OnHitFX(){
        // SFX
        onHitAudio.PlayOnce();
      
        DestroyBullet();
    }
}
