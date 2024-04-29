using Godot;
using System;

public partial class Gun : RigidBody2D, IShipComponent, IPowerable
{
	[Export]
    private DefenseInfo defenseInfo;

	[Export]
	private int powerdraw;

    [Export]
    private PackedScene ammo;

    [Export]
    private DamageInfo damageInfo;

    [Export]
    private float bulletSpeed;

    public override void _Ready()
    {
        Shoot();
    }

    public void Shoot()
    {
        Bullet bullet = ammo.Instantiate() as Bullet;
        this.AddChild(bullet);
        bullet.damageInfo = damageInfo;
        bullet.ConstantForce = Vector2.Up * 100;
        GD.Print(bullet.ConstantForce);
    }

    public DefenseInfo GetDefenseInfo() => defenseInfo;

    public int GetPowerDraw() => powerdraw;
}
