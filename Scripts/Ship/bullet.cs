using Godot;
using System;

public partial class Bullet : RigidBody2D
{
	public DamageInfo damageInfo {get; set;}

	public float speed {get; set;} = 5;

    public void Shoot()
    {
        this.LinearVelocity = Vector2.Up * speed;
    }
}
