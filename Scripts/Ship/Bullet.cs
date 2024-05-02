using Godot;
using System;

public partial class Bullet : CharacterBody2D, IDamager
{
	[Export]
	private Timer timer;

	[Export]
	private double lifeTime = 10;

	public float speed {get; set;}

	private DamageInfo damageInfo;

    public DamageInfo GetDamageInfo() => damageInfo;

    public void SetDamageInfo(DamageInfo damageInfo) { this.damageInfo = damageInfo; }

    public override void _Ready()
    {
        timer.Start(lifeTime);
		timer.Timeout += this.QueueFree;
    }

    public override void _Process(double delta)
    {
        MoveAndCollide(new Vector2(0, (float)(delta * (-speed * 1 ))));
    }
}
