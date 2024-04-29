using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export]
	private Timer timer;

	[Export]
	private double lifeTime = 10;

	public float speed {get; set;}

	public DamageInfo damageInfo {get; set;}

    public override void _Ready()
    {
        timer.Start(lifeTime);
		timer.Timeout += this.QueueFree;
    }

    public override void _Process(double delta)
    {
        Position = new Vector2(Position.X, Position.Y + ((float)delta * -speed));
    }

    public void Hit(Node node)
	{
		if(node is IHittable)
		{
			IHittable hittable = node as IHittable;
			hittable.TakeDamage(damageInfo);
		}
		this.QueueFree();
	}
}
