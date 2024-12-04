using Godot;

public partial class Gun : ShipComponent, IPowerable
{
	[Export]
	private Timer timer;

	[Export]
	private AttackComponent attackComponent;
	[Export]
	public GunType type { get; private set; }
	[Export]
	public Texture2D gunTypeIcon {get; private set;}

	[Export]
	private Node2D bulletSpawnPoint;
	[Export]
	private int powerdraw;
	private PackedScene bulletPrefab;

	private float bulletSpeed;
	private double bulletsPerSecond = 1;
	private float bulletSpawnOffset = 32;
	private bool canShoot = true;
	private bool isShootingPressed = false;

	public override void _Ready()
	{
		base._Ready();
		timer.Stop();
		canShoot = true;
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

		Bullet bullet = bulletPrefab.Instantiate() as Bullet;

		bullet.GlobalPosition = bulletSpawnPoint.GlobalPosition;
		bullet.GlobalRotation = GlobalRotation;
		bullet.SetAttackInfo(attackComponent);
		bullet.speed = bulletSpeed;
		GetTree().CurrentScene.AddChild(bullet);
		timer.Start(1 / bulletsPerSecond);
	}

	public int GetPowerDraw() => powerdraw;

	public override void SetupData(ShipComponentData data)
	{
		base.SetupData(data);
		if (data is GunData gunData)
		{
			attackComponent.attack = gunData.Attack;
			powerdraw = gunData.Powerdraw;
			bulletsPerSecond = gunData.BulletsPerSecond;
			bulletSpeed = gunData.BulletSpeed;
			bulletSpawnPoint.Position = gunData.BulletSpawnPoint;
			if (IsMirrored) { bulletSpawnPoint.Position *= Vector2.Left + Vector2.Down; }
			bulletPrefab = gunData.BulletPrefab;
			type = gunData.type;
			gunTypeIcon = gunData.GunTypeIcon;
		}
	}
}
