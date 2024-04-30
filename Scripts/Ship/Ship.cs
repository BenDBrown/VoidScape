using Godot;
using System;
using System.Collections.Generic;

public partial class Ship : Node2D
{
	//in seconds
	private const int POWER_TICK_RATE = 1;

	public int fuelCapacity {get; private set;} = 0;

 	[Export]
	private PowerManager powerManager;

	[Export]
	private CargoManager cargoManager;

	[Export]
	private Timer timer;

	private List<ShipComponent> shipComponents = new();

	private List<Gun> guns = new();

	private Dictionary<IPowerable, bool> powerables = new();

	private bool stalling = false;

	private int fuel = 0;


    public override void _Ready()
    {
		timer.Start(POWER_TICK_RATE);
		timer.Timeout += PowerTick;
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

	public void Shoot() 
	{ 
		if(!stalling) 
		{
			foreach(Gun gun in guns) 
			{ 
				UsePowerable(gun);
				gun.Shoot(); 
			} 
		} 
	}

		private bool TryBuildShip()
	{
		bool hasFuelTank = false;
		bool hasGenerator = false;
		bool hasThruster = false;
		bool thrusterPowerUsageUnderMaxPower = false;
		int thrustPowerNeeded = 0;
		int maxPowerGenerated;
		int cargoCapacity = 0;
		
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
					hasThruster = true;
					break;
				default: break;
			}
			if(node is IPowerable) { powerables.Add(node as IPowerable, false); }
			if(node is ShipComponent) 
			{ 
				ShipComponent shipComponent = node as ShipComponent;
				shipComponents.Add(shipComponent); 
				shipComponent.OnDestroyed += ComponentDestroyed;
			}
			cargoManager.cargoCapacity = cargoCapacity;

			maxPowerGenerated = powerManager.GetMaxPowerGenerated();
			thrusterPowerUsageUnderMaxPower = maxPowerGenerated >= thrustPowerNeeded;
		}
		return hasFuelTank && hasGenerator && hasThruster && thrusterPowerUsageUnderMaxPower;
	}

	private void PowerTick()
	{
		if(stalling){ stalling = false; }
		int stallTimer = 5;
		int powerWanted = 0;
		foreach(IPowerable powerable in powerables.Keys)
		{
			if(powerables[powerable]) { powerWanted += powerable.GetPowerDraw(); powerables[powerable] = false;}
		}
		powerManager.TryUsePower(powerWanted, fuel, out int fuelUsed, out bool enoughPower, out bool enoughFuel);
		fuel -= fuelUsed;
		if(fuel < 0) {fuel = 0;}
		if(!enoughFuel) { ShipDestroyed(); }
		if(enoughPower) { timer.Start(POWER_TICK_RATE); }
		else
		{ 
			stalling = true;
			timer.Start(stallTimer);
		}
	}

	private void UsePowerable(IPowerable powerable)
	{
		if(powerables.ContainsKey(powerable))
		{
			powerables[powerable] = true;
		}
		else { GD.Print("powerable was not in powerable list"); }
	}
	
	public bool TryAddCargo(Cargo cargo, int quantity, out int cargoAdded) 
	{ 
		bool success = cargoManager.TryAddCargo(cargo, quantity, out int cargoAddedToManager); 
		cargoAdded = cargoAddedToManager;
		return success;
	}

	public bool TryTakeCargo(Cargo cargo, int quantity) { return cargoManager.TryTakeCargo(cargo, quantity); }

}
