using Godot;
using System;

public partial class Turret : Node2D
{
	[ExportCategory("Turret Configuration")]
	[Export]
	private Node2D gun;
	[Export]
	private RayCast2D raycast;
	[Export]
	private float rayCastRange = 300;
	[Export] 
	private Line2D laserLine;
	[Export]
	private AnimatedSprite2D deathAnimation;
	[Export]
	private AnimationPlayer idleAnimation;

	[ExportCategory("Bullet Configuration")]
	[Export]
	private PackedScene bulletPrefab;
	[Export]
	private float bulletSpeed;
	[Export]
	private float bulletsPerSecond;
	[Export]
	private Node2D leftBulletSpawnPoint;
	[Export]
	private Node2D rightBulletSpawnPoint;

	[ExportCategory("Shooting Animation Configuration")]
	[Export]
	private AnimatedSprite2D gunSprite;
	[Export]
	private string shootAnimationName = "shoot";
	[Export]
	private int shootAnimationFrame = 0;
	[Export]
	private SingleRunAudio shootSFX;

	[ExportCategory("General Components")]
	[Export]
	private AttackComponent attackComponent;
	[Export]
	private Timer timer;
	[Export]
	private Node healthComponent;

	private PlayerShip playerShip;
	private bool canShoot = true;
	private bool isDestroyed = false;
	private bool isShooting = false;
	private bool isIdle = true;
	private bool targetInSight = false;

	public override void _Ready()
	{
		playerShip = Game.Instance.PlayerShip;

        if (healthComponent != null && healthComponent.HasSignal("died"))
        {
            healthComponent.Connect("died", Callable.From(Destroyed));
        }

		canShoot = true;
		timer.Timeout += () => canShoot = true;
		timer.Timeout += StartShooting;

		SetupShootingAnimation();
		
		// Setting the distance of the raycast that tracks the playership
		Vector2 trackerRangeV2 = new Vector2{X = rayCastRange};
		raycast.TargetPosition = trackerRangeV2;

		laserLine.AddPoint(trackerRangeV2);
	}

	public override void _Process(double delta)
	{
		if(isShooting){
			if(IsShootingAnimFrame(gunSprite.Frame)){
				Shoot();
			}
		}
	}

    public override void _PhysicsProcess(double delta)
    {
        if(playerShip == null){ return; }
		if(isDestroyed) { return;}

		if(!isIdle && !targetInSight){
			isIdle = true;

			if(idleAnimation != null){
				idleAnimation.Play();
			}
		}

		UpdateLaserDistance();
		
		if(targetInSight){
			LockOnTarget();
			StartShooting();
		}
		else{
			LookForPlayer();
			StopShooting();
		}
    }

    private void StartShooting(){
		if(isDestroyed) { return;}
		if(!canShoot) {return;}
		canShoot = false;
		
		isShooting = true;
		gunSprite.Play();	
	}

	private void StopShooting(){
		if(!targetInSight){
			gunSprite.Stop();
		}
	}

	private void Shoot(){
		Bullet bulletLeft = bulletPrefab.Instantiate() as Bullet;
		Bullet bulletRight = bulletPrefab.Instantiate() as Bullet;

		SetBulletInfo(bulletLeft, leftBulletSpawnPoint);
		SetBulletInfo(bulletRight, rightBulletSpawnPoint);

		// Play Shooting Audio
		shootSFX.PlayOnce();
		isShooting = false;
	}

	private void SetBulletInfo(Bullet bullet, Node2D bulletSpawnPoint){
		bullet.GlobalPosition = bulletSpawnPoint.GlobalPosition;
		bullet.GlobalRotation = bulletSpawnPoint.GlobalRotation;

		bullet.SetAttackInfo(attackComponent);

		bullet.speed = bulletSpeed;
		GetTree().CurrentScene.AddChild(bullet);
		timer.Start(1 / bulletsPerSecond);
	}

	private void LockOnTarget(){
		float angleToTarget = gun.GlobalPosition.DirectionTo(playerShip.GlobalPosition).Angle();
		raycast.GlobalRotation = angleToTarget;
		gun.GlobalRotation = angleToTarget;

		targetInSight = IsTargetInSight();
	}

	private void UpdateLaserDistance(){
		if(raycast.IsColliding()){
			Vector2 collisionPoint = raycast.GetCollisionPoint();
			laserLine.SetPointPosition(1, laserLine.ToLocal(collisionPoint));
		}
		else{
			laserLine.SetPointPosition(1, raycast.TargetPosition);
		}
	}

	private bool IsShootingAnimFrame(int frame){
		return shootAnimationFrame == frame;
	}

	private void SetupShootingAnimation(){
		// Setup Shooting Animation Speed
		int totalShootingFrames = gunSprite.SpriteFrames.GetFrameCount(shootAnimationName);
		double frameFPS = totalShootingFrames * bulletsPerSecond;
		gunSprite.SpriteFrames.SetAnimationSpeed(shootAnimationName ,frameFPS);
	
		gunSprite.AnimationFinished += StopShooting;
	}

	private void Destroyed(){
		if(isDestroyed) {return;}

		deathAnimation.Visible = true;
		deathAnimation.AnimationFinished += DisableDeathAnimation;
		deathAnimation.Play();

		isDestroyed = true;
		gun.QueueFree();
	}

	private void DisableDeathAnimation(){
		deathAnimation.Visible = false;
	}

	private void LookForPlayer(){
		if(IsTargetInSight()){
			targetInSight = true;

			isIdle = false;
			if(idleAnimation != null){
				idleAnimation.Pause();
			}
		}
	}

	private bool IsTargetInSight(){
		return raycast.IsColliding() && raycast.GetCollider() is PlayerShip;
	}
}
