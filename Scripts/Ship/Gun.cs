using Godot;
using System;

public partial class Gun : Node2D, IShipComponent, IPowerable
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
        var bulletNode = ammo.Instantiate();
        if(bulletNode is Bullet){GD.Print("is bullet");}
        // bullet.damageInfo = damageInfo;
        // bullet.speed = bulletSpeed;
        // bullet.Shoot();
    }

    public DefenseInfo GetDefenseInfo() => defenseInfo;

    public int GetPowerDraw() => powerdraw;
}
