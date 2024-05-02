using Godot;
using System;
using System.Collections.Generic;

public partial class NPCShip : CharacterBody2D, IShip
{
	private const float THRUST_WEIGHT_MOD = 0.3f;

	private const float THRUST_BACK_MOD = 0.5f;

	private const float MAX_ROTATION_SPEED = 3;

	[Export]
	private CollisionPolygon2D collider;

	private List<ShipComponent> shipComponents = new();

	private List<Gun> guns = new();

	private List<Thruster> thrusters= new();

	private bool shooting = false;

	private ThrustDirection thrustDirection = ThrustDirection.none;

	private float thrust;

	private float rotationDirection = 0;

	private float rotationSpeed;

	public override void _Ready()
	{
		TryBuildShip();
	}

	public void ShipDestroyed()
	{
		GD.Print("ship destroyed");
	}

	public void ComponentDestroyed(ShipComponent shipComponent)
	{
		// call death check and update relevant values here
		GD.Print(shipComponent.Name + " destroyed");
	}

		public override void _PhysicsProcess(double delta)
	{
		if(thrustDirection != ThrustDirection.none) 
		{
			Vector2 thrustVector;
			thrustVector = thrustDirection == ThrustDirection.forward ? -Transform.Y : Transform.Y;
			Vector2 moveVector = thrustVector * (float)(thrust * delta);
			MoveAndCollide(moveVector);
			MoveAndSlide();
		}
		if(shooting)
		{
			StopShooting();
		}
		if(rotationDirection != 0)
		{
			Rotation += (float)(rotationDirection * rotationSpeed * delta);
		}
	}

	public void StartShooting() 
	{ 
		if(shooting) { return; }
		foreach(Gun gun in guns) 
		{ 
			gun.StartShooting();
		}
		shooting = true;
	}

	public void StopShooting() 
	{
		shooting = false; 
		foreach(Gun gun in guns) { gun.StopShooting(); }
	}

	public void ForwardThrust()
	{
		CalculateThrust();
		thrustDirection = ThrustDirection.forward;
	}

	public void BackThrust()
	{
		CalculateThrust();
		thrust *= THRUST_BACK_MOD;
		thrustDirection = ThrustDirection.back;	
	}

	public void StopThrusting() { thrustDirection = ThrustDirection.none; }

	public void StartTurningClockwise()
	{
		rotationDirection = 1;
		CalculateThrust();
		rotationSpeed = thrust;
		if(rotationSpeed > MAX_ROTATION_SPEED) { rotationSpeed = MAX_ROTATION_SPEED; }
	}
	
	public void StartTurningCounterClockwise()
	{
		rotationDirection = -1;
		CalculateThrust();
		rotationSpeed = thrust;
		if(rotationSpeed > MAX_ROTATION_SPEED) { rotationSpeed = MAX_ROTATION_SPEED; }
	}

	public void StopTurning() { rotationDirection = 0; }

	public bool TryBuildShip()
	{
		bool hasFuelTank = false;
		bool hasGenerator = false;
		bool hasThruster = false;
		
		List<Vector2> unpackedVectors = new();

		foreach(Node node in GetChildren())
		{
			switch(node)
			{
				case Gun gun:
					guns.Add(gun);
					break;
				case FuelTank fuelTank:
					hasFuelTank = true;
					break;
				case Generator generator:
					hasGenerator = true; 
					break;
				case Thruster thruster:
					thrusters.Add(thruster);
					hasThruster = true;
					break;
				default: break;
			}
			if(node is ShipComponent) 
			{ 
				ShipComponent shipComponent = node as ShipComponent;
				shipComponents.Add(shipComponent); 
				shipComponent.OnDestroyed += ComponentDestroyed;
				
				foreach (Vector2 v2 in shipComponent.GetVertices())
				{ 
					Vector2 localPositon = ToLocal(v2);
					unpackedVectors.Add(localPositon);
				}
			}
			
		}
		ConvexPolygonShape2D convexPolygon = new ();
		convexPolygon.SetPointCloud(unpackedVectors.ToArray());
		collider.Polygon = convexPolygon.Points;

		return hasFuelTank && hasGenerator && hasThruster;
	}

	private void CalculateThrust()
	{
		thrust = 0;
		foreach (Thruster thruster in thrusters)
		{
			thrust += thruster.GetThrust();
		}
		thrust /= shipComponents.Count * THRUST_WEIGHT_MOD;
	}

}
