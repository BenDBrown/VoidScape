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
    private Timer timer;

    [Export]
    private float bulletSpeed;

    [Export] // lower values = faster
    private double fireInterval = 1;

    public override void _Ready()
    {
		timer.Timeout += Shoot;
    }

    public void StartShooting() { Shoot(); }

    public void StopShooting(){ timer.Stop(); }

    public void Shoot()
    {
        Bullet bullet = ammo.Instantiate() as Bullet;
        this.AddChild(bullet);
        bullet.SetDamageInfo(damageInfo);
        bullet.speed = bulletSpeed;
        timer.Start(fireInterval);
    }

    public int GetPowerDraw() => powerdraw;
}
