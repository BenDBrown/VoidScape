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

    private int bulletSpawnOffset = -32;

    public override void _Ready()
    {
        timer.Timeout += Shoot;
    }

    public void StartShooting() { Shoot(); }

    public void StopShooting(){ timer.Stop(); }

    public void Shoot()
    {
        Bullet bullet = ammo.Instantiate() as Bullet;
        bullet.GlobalPosition = GlobalPosition;
        bullet.GlobalRotation = GlobalRotation;
        bullet.Position += Vector2.FromAngle(bullet.GlobalRotation + 1.5f) * bulletSpawnOffset;
        bullet.SetDamageInfo(damageInfo);
        bullet.speed = bulletSpeed;
        GetTree().CurrentScene.AddChild(bullet);
        timer.Start(fireInterval);
    }

    public int GetPowerDraw() => powerdraw;
}
