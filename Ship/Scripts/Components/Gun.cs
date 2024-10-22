using Godot;
using System;

public partial class Gun : ShipComponent, IPowerable
{
	[Export]
	private int powerdraw;

	[Export]
	private PackedScene ammo;

	[Export]
	private AttackComponent attackComponent;

	[Export]
	private Timer timer;

	[Export]
	private float bulletSpeed;

	[Export] // lower values = faster
	private double fireInterval = 1;

	[Export]
	public GunType type { get; private set; }

	private int bulletSpawnOffset = -32;
	private bool canShoot = true;
	private bool isShootingPressed = false;

	public override void _Ready()
	{
		timer.Timeout += () => canShoot = true;
		timer.Timeout += Shoot;
	}

	public void StartShooting() { isShootingPressed = true; Shoot(); }

	public void StopShooting() { isShootingPressed = false; }

	public void Shoot()
	{
		if (IsDestroyed()) { return; }
		if (!canShoot || !isShootingPressed) { return; }

		canShoot = false;
		Bullet bullet = ammo.Instantiate() as Bullet;
		bullet.GlobalPosition = GlobalPosition;
		bullet.GlobalRotation = GlobalRotation;
		bullet.Position += Vector2.FromAngle(bullet.GlobalRotation + 1.5f) * bulletSpawnOffset;
		bullet.SetAttackInfo(attackComponent);
		bullet.speed = bulletSpeed;
		GetTree().CurrentScene.AddChild(bullet);
		timer.Start(fireInterval);
	}

	public int GetPowerDraw() => powerdraw;
}
