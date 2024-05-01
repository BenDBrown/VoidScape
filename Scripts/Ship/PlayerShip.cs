using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerShip : CharacterBody2D, IShip
{
	private const float THRUST_WEIGHT_MOD = 0.3f;

	private const float THRUST_BACK_MOD = 0.5f;

	private const float MAX_ROTATION_SPEED = 3;

	public int fuelCapacity {get; private set;} = 0;

	[Export]
	private CollisionPolygon2D collider;

 	[Export]
	private PowerManager powerManager;

	[Export]
	private CargoManager cargoManager;

	private List<ShipComponent> shipComponents = new();

	private List<Gun> guns = new();

	private List<Thruster> thrusters= new();

	private bool shooting = false;

	private float shootPowerDraw;

	private ThrustDirection thrustDirection = ThrustDirection.none;

	private float thrust;

	private float thrustPowerDraw;

	private float fuel = 0;

	private float rotationDirection = 0;

	private float rotationSpeed;


    public override void _Ready()
    {
		TryBuildShip();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(thrustDirection != ThrustDirection.none) 
		{
			if(TryUsePowerable((float)(thrustPowerDraw * delta)))
			{
				Vector2 thrustVector;
				thrustVector = thrustDirection == ThrustDirection.forward ? -Transform.Y : Transform.Y;
				Vector2 moveVector = thrustVector * (float)(thrust * delta);
				MoveAndCollide(moveVector);
				MoveAndSlide();
			}
			else { thrustDirection = ThrustDirection.none; }
		}
		if(shooting && !TryUsePowerable((float)(shootPowerDraw * delta)))
		{
			StopShooting();
		}
		if(rotationDirection != 0)
		{
			Rotation += (float)(rotationDirection * rotationSpeed * delta);
		}
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

	public void StartShooting() 
	{ 
		shootPowerDraw = 0;
		foreach(Gun gun in guns) 
		{ 
			shootPowerDraw += gun.GetPowerDraw();
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
		bool thrusterPowerUsageUnderMaxPower = false;
		int thrustPowerNeeded = 0;
		float maxPowerGenerated;
		int cargoCapacity = 0;
		
		List<Vector2> unpackedVectors = new();

        foreach(Node node in GetChildren())
		{
			switch(node)
			{
				case Gun gun:
					guns.Add(gun);
					break;
				case Hull hull:
					cargoCapacity += hull.cargoCapacity;
					break;
				case FuelTank fuelTank:
					fuelCapacity += fuelTank.fuelCapacity; 
					hasFuelTank = true;
					break;
				case Generator generator:
					powerManager.AddGenerator(generator);
					hasGenerator = true; 
					break;
				case Thruster thruster:
					thrustPowerNeeded += thruster.GetPowerDraw();
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
		

		cargoManager.cargoCapacity = cargoCapacity;
		maxPowerGenerated = powerManager.GetMaxPowerGenerated();
		thrusterPowerUsageUnderMaxPower = maxPowerGenerated >= thrustPowerNeeded;
		fuel = fuelCapacity; // remove this line later so that fuel doesnt reset when a ship is re-instantiated

		return hasFuelTank && hasGenerator && hasThruster && thrusterPowerUsageUnderMaxPower;
	}

	private void CalculateThrust()
	{
		thrust = 0;
		thrustPowerDraw = 0;
		foreach (Thruster thruster in thrusters)
		{
			thrustPowerDraw += thruster.GetPowerDraw();
			thrust += thruster.GetThrust();
		}
		thrust /= shipComponents.Count * THRUST_WEIGHT_MOD;
	}

	private bool TryUsePowerable(float powerWanted)
	{
		if(powerManager.stalling) { return false; }
		bool result = powerManager.TryUsePower(powerWanted, fuel, out float fuelUsed, out bool hasEnoughFuel);
		if(!hasEnoughFuel){ShipDestroyed(); return false; }
		fuel -= fuelUsed;
		return result;
	}
	
	public bool TryAddCargo(Cargo cargo, int quantity, out int cargoAdded) 
	{ 
		return cargoManager.TryAddCargo(cargo, quantity, out cargoAdded); 
	}

	public bool TryTakeCargo(Cargo cargo, int quantity) { return cargoManager.TryTakeCargo(cargo, quantity); }


}

