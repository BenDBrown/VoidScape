using Godot;
using System;
using System.Collections.Generic;

public partial class Ship : CharacterBody2D
{


	private const float THRUST_WEIGHT_MOD = 0.3f;

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

	private bool thrusting = false;

	float thrust;

	private float fuel = 0;


    public override void _Ready()
    {
		TryBuildShip();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(thrusting) 
		{
			Vector2 moveVector = new Vector2(0, (float)(delta * (thrust * -1)));
			MoveAndCollide(moveVector);
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

	public void Shoot() 
	{ 
		foreach(Gun gun in guns) 
		{ 
			if(TryUsePowerable(gun)) { gun.Shoot();  }
		} 
	}

	public void ForwardThrust()
	{
		GD.Print("thrusting");
		thrust = 0;
		foreach (Thruster thruster in thrusters)
		{
			if(TryUsePowerable(thruster)) { thrust += thruster.GetThrust(); }
			else{GD.Print("couldnt thrust");}
		}
		thrust /= shipComponents.Count * THRUST_WEIGHT_MOD;
		thrusting = true;
	}

	public void StopThrusting() { GD.Print("not thrusting"); thrusting = false; }


	private bool TryBuildShip()
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

				foreach (Vector2 v2 in shipComponent.GetVertices()){ unpackedVectors.Add(v2); GD.Print("vector: " + v2.ToString()); }
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
	

	private bool TryUsePowerable(IPowerable powerable)
	{
		if(powerManager.stalling) { return false; }
		powerManager.TryUsePower(powerable.GetPowerDraw(), fuel, out float fuelUsed, out bool enoughPower, out bool enoughFuel);
		GD.Print("power wanted: " + powerable.GetPowerDraw() + ", fuel: " + fuel + ", fuel used: " + fuelUsed + ", enoughPower: " + enoughPower + ", enoughFuel: " + enoughFuel);
		if(!enoughFuel){ShipDestroyed(); return false; }
		else if(!enoughPower) {powerManager.stalling = true;}
		fuel -= fuelUsed;
		return enoughPower && enoughFuel;
	}
	
	public bool TryAddCargo(Cargo cargo, int quantity, out int cargoAdded) 
	{ 
		bool success = cargoManager.TryAddCargo(cargo, quantity, out int cargoAddedToManager); 
		cargoAdded = cargoAddedToManager;
		return success;
	}

	public bool TryTakeCargo(Cargo cargo, int quantity) { return cargoManager.TryTakeCargo(cargo, quantity); }

}
