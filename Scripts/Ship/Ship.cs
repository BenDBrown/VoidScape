using Godot;
using System;
using System.Collections.Generic;

public partial class Ship : Node2D
{
	public int cargoCapacity {get; private set;} = 0;

	public int fuelCapacity {get; private set;} = 0;

	private List<ShipComponent> shipComponents = new();

	private List<Gun> guns = new();

	private PowerManager powerManager = new PowerManager();

	

    public override void _Ready()
    {
		bool hasFuelTank = false;
		bool hasGenerator = false;
        foreach(Node node in this.GetChildren())
		{
			if(node is Gun){guns.Add(node as Gun); }
			else if(node is Hull) { Hull hull = node as Hull; cargoCapacity += hull.cargoCapacity; }
			else if(node is FuelTank) { FuelTank fuelTank = node as FuelTank; fuelCapacity += fuelTank.fuelCapacity; hasFuelTank = true;}
			else if(node is Generator) { powerManager.AddGenerator(node as Generator); hasGenerator = true; }
			if(node is ShipComponent) 
			{ 
				ShipComponent shipComponent = node as ShipComponent;
				shipComponents.Add(shipComponent); 
				shipComponent.OnDestroyed += ComponentDestroyed;
			}
			
		}
    }

	public void ComponentDestroyed(ShipComponent shipComponent)
	{
		// call death check and update relevant values here
		GD.Print(shipComponent.Name + " destroyed");
	}

	public void Shoot() { foreach(Gun gun in guns) { gun.Shoot(); } }
	
}
