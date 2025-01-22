using Godot;
using System;

public partial class Turret : Node2D
{
	[Export]
	private Node2D gun;

	[ExportCategory("Idle Animation Configuration")]
	[Export]
	private float minRotationDegrees = 0f;
	[Export]
	private float maxRotationDegrees = 180f;
	[Export]
	private float rotationSpeed = 1f;

	[ExportCategory("Raycast")]
	[Export]
	private RayCast2D raycast;
	[Export]
	private float turretFollowSpeed = 5f;
	[Export]
	private Line2D laserLine;
	[Export]
	private float rayCastRange = 300f;
	[Export]
	private Color greenLaser;
	[Export]
	private Color redLaser;

	[ExportCategory("Shooting Animation Configuration")]
	[Export]
	private AnimatedSprite2D gunSprite;
	[Export]
	private string shootAnimationName = "shoot";
	[Export]
	private int shootAnimationFrame = 0;
	[Export]
	private SingleRunAudio shootSFX;

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

	[ExportCategory("General")]
	[Export]
	private AnimatedSprite2D deathAnimation;
	[Export]
	private AttackComponent attackComponent;
	[Export]
	private Timer timer;
	[Export]
	private Node healthComponent;
	[Export]
	private PackedScene turretBasePrefab;
	[Export]
	private Sprite2D turretBase; 

	// Idle Animation
	private float elapsedTime = 0f;
	private bool isIdle = true;

	// General
	private bool isDestroyed = false;
	
	private PlayerShip playerShip;

	// Shooting
	private bool isShooting = false;
	private bool canShoot = true;
	private bool targetIsInSight = false;
	private bool isFollowingPlayer = false;
	private float minRotation;
	private float maxRotation;

    public override void _Ready()
    {
        playerShip = Game.Instance.PlayerShip;

        if (healthComponent != null && healthComponent.HasSignal("died"))
        {
            healthComponent.Connect("died", Callable.From(Destroyed));
        }

		SetupTimer();
		SetupRaycastAndLaser();
		SetupShootingAnimation();

		minRotation = Mathf.DegToRad(minRotationDegrees);
		maxRotation = Mathf.DegToRad(maxRotationDegrees);
    }

    public override void _PhysicsProcess(double delta)
    {
		if(isDestroyed) { return;}

		if(isShooting){
			if(IsShootingAnimFrame(gunSprite.Frame)){
				Shoot();
			}
		}

		targetIsInSight = IsTargetInSight();

		if(targetIsInSight && isIdle){
			canShoot = true;
		}

		if(targetIsInSight){
			LockOnTarget(delta);
			laserLine.DefaultColor = redLaser;
			StartShooting();
			isIdle = false;
		}
		
		if(isIdle){
			PlayIdleAnimation((float)delta);
		}
		else{

			if(!targetIsInSight){
				isIdle = true;

				laserLine.DefaultColor = greenLaser;
				StopShooting();
				StartIdleAnimationFromCurrentPosition();
			}
		}

		UpdateLaserDistance();
    }

    private void PlayIdleAnimation(float delta){
		elapsedTime += delta * rotationSpeed;

		// Convert min and max rotations to radians
        float minRotation = Mathf.DegToRad(minRotationDegrees);
        float maxRotation = Mathf.DegToRad(maxRotationDegrees);

        // Calculate the rotation using sine
        float t = Mathf.Sin(elapsedTime); // Oscillates between -1 and 1
        float rotation = Mathf.Lerp(minRotation, maxRotation, (t + 1.0f) / 2.0f); // Map -1..1 to 0..1 and interpolate

        // Apply the rotation to the node
        gun.GlobalRotation = rotation;
	}

	private void StartIdleAnimationFromCurrentPosition(){

		float normalizedRotation = Mathf.InverseLerp(minRotation, maxRotation, gun.GlobalRotation);
		float sinePhase = Mathf.Asin(2 * normalizedRotation - 1); // Map 0..1 to sine phase (-1..1)

		// Determine the nearest extreme and adjust the sine phase to move in that direction
		float midPoint = (minRotation + maxRotation) / 2.0f;
		if (gun.GlobalRotation < midPoint)
		{
			// Closer to min rotation, adjust sine phase to move toward it
			sinePhase = Mathf.Max(sinePhase, -Mathf.Pi / 2); // -1 on sine wave
		}
		else
		{
			// Closer to max rotation, adjust sine phase to move toward it
			sinePhase = Mathf.Min(sinePhase, Mathf.Pi / 2); // +1 on sine wave
		}

		elapsedTime = sinePhase / rotationSpeed;
	}

	private bool IsTargetInSight(){
		return raycast.IsColliding() && raycast.GetCollider() is PlayerShip;
	}
	
	private void LockOnTarget(double delta){
		float currentRotation = gun.GlobalRotation;		
		float angleToTarget = gun.GlobalPosition.DirectionTo(playerShip.GlobalPosition).Angle();
		
		gun.GlobalRotation = Mathf.LerpAngle(currentRotation, angleToTarget, turretFollowSpeed * (float)delta);
		raycast.GlobalRotation = gun.GlobalRotation;
	}

	private void SetupRaycastAndLaser(){
		// Setting the distance of the raycast that tracks the playership
		Vector2 trackerRangeV2 = new Vector2{X = rayCastRange};
		raycast.TargetPosition = trackerRangeV2;

		laserLine.AddPoint(trackerRangeV2);
		laserLine.DefaultColor = greenLaser;
	}

	private void Destroyed(){
		if(isDestroyed) {return;}

		deathAnimation.Visible = true;
		deathAnimation.AnimationFinished += DespawnTurret;
		deathAnimation.Play();

		gun.QueueFree();
		isDestroyed = true;
	}

	private void DespawnTurret(){
		// Create a turretbase object and place it separately in the scene as a remnant of the 
		
		Sprite2D newTurretBase = turretBasePrefab.Instantiate() as Sprite2D;
		newTurretBase.GlobalTransform = turretBase.GlobalTransform;
		newTurretBase.ZIndex = turretBase.ZIndex;
		GetTree().CurrentScene.AddChild(newTurretBase);
		
		QueueFree();
	}

	private bool IsShootingAnimFrame(int frame){
		return shootAnimationFrame == frame;
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

	private void UpdateLaserDistance(){
		if(raycast.IsColliding()){
			Vector2 collisionPoint = raycast.GetCollisionPoint();
			laserLine.SetPointPosition(1, laserLine.ToLocal(collisionPoint));
		}
		else{
			laserLine.SetPointPosition(1, raycast.TargetPosition);
		}
	}

    private void StartShooting(){
		if(isDestroyed) { return;}
		if(!canShoot) {return;}
		if(!targetIsInSight) {return;}
		canShoot = false;
		
		isShooting = true;
		gunSprite.Play();	
	}

	private void StopShooting(){
		if(!targetIsInSight){
			gunSprite.Stop();
			timer.Stop();
		}
	}

	private void SetupTimer(){
		canShoot = true;
		timer.Timeout += () => canShoot = true;
		timer.Timeout += StartShooting;
	}

	private void SetupShootingAnimation(){
		// Setup Shooting Animation Speed
		int totalShootingFrames = gunSprite.SpriteFrames.GetFrameCount(shootAnimationName);
		double frameFPS = totalShootingFrames * bulletsPerSecond;
		gunSprite.SpriteFrames.SetAnimationSpeed(shootAnimationName ,frameFPS);
	
		gunSprite.AnimationFinished += StopShooting;
	}
}
