using Godot;
using System;

public partial class Gun : ShipComponent, IPowerable
{
	[Export]
	private int powerdraw;

    [Export]
    private PackedScene ammo;

    [Export]
    private DamageInfo damageInfo;

    [Export]
    private float bulletSpeed;

    public void Shoot()
    {
        Bullet bullet = ammo.Instantiate() as Bullet;
        this.AddChild(bullet);
        bullet.damageInfo = damageInfo;
        bullet.speed = bulletSpeed;
    }

    public int GetPowerDraw() => powerdraw;
}
