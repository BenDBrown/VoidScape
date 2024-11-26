using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PlayerShip : Ship, IShip
{
	private PowerManager powerManager;
	private CargoManager cargoManager;
	private FuelManager fuelManager;

	public override bool TryBuildShip()
	{
		bool hasFuelTank = false;
		bool hasGenerator = false;
		bool hasThruster = false;

		List<Vector2> globalVertices = new();

		foreach(Node node in GetChildren())
		{
			if (node is not ShipComponent shipComponent) { continue; }
			switch(node)
			{
				case Gun gun:
					gunManager.AddGun(gun);
					break;
				case Hull hull:
					cargoManager.AddHull(hull);
					break;
				case FuelTank fuelTank:
					fuelManager.AddFuelTank(fuelTank);
					hasFuelTank = true;
					break;
				case Generator generator:
					powerManager.AddGenerator(generator);
					hasGenerator = true;
					break;
				case Thruster thruster:
					thrustManager.AddThruster(thruster);
					hasThruster = true;
					break;
				default: break;
			}
			shipComponents.Add(shipComponent);
			shipComponent.OnDestroyed += ComponentDestroyed;
			globalVertices.Add(shipComponent.GlobalPosition);
		}

		Vector2 center = centerCalculator.GetGlobalShipCenter(globalVertices);
		foreach (Node n in GetChildren())
		{
			if (n is Camera2D) { continue; }
			if (n is Node2D n2 && n is not SingleRunAnimation) { n2.Position -= ToLocal(center); } // is not, for bandaid solution to prevent ship destruction anim from being off centre
			if (n is ShipComponent shipComponent)
			{
				shipComponent.collider.Owner = null; //prevents warning.
				shipComponent.collider.Reparent(this);
				shipComponent.collider.Owner = this;
			}
		}
		thrustManager.SetWeight(shipComponents.Count);


		return hasFuelTank && hasGenerator && hasThruster;
	}

}
