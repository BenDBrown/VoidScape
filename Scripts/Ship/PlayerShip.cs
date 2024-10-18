using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class PlayerShip : Ship, IShip
{
	// public bool TryBuildShip()
	// {
	// 	bool hasFuelTank = false;
	// 	bool hasGenerator = false;
	// 	bool hasThruster = false;
	// 	bool thrusterPowerUsageUnderMaxPower = false;
	// 	int thrustPowerNeeded = 0;
	// 	float maxPowerGenerated;
	// 	int cargoCapacity = 0;
		
	// 	List<Vector2> unpackedVectors = new();

	// 	foreach(Node node in GetChildren())
	// 	{
	// 		switch(node)
	// 		{
	// 			case Gun gun:
	// 				guns.Add(gun);
	// 				break;
	// 			case Hull hull:
	// 				cargoCapacity += hull.cargoCapacity;
	// 				break;
	// 			case FuelTank fuelTank:
	// 				fuelCapacity += fuelTank.fuelCapacity; 
	// 				hasFuelTank = true;
	// 				break;
	// 			case Generator generator:
	// 				powerManager.AddGenerator(generator);
	// 				hasGenerator = true; 
	// 				break;
	// 			case Thruster thruster:
	// 				thrustPowerNeeded += thruster.GetPowerDraw();
	// 				thrusters.Add(thruster);
	// 				hasThruster = true;
	// 				break;
	// 			default: break;
	// 		}
	// 		if(node is ShipComponent) 
	// 		{ 
	// 			ShipComponent shipComponent = node as ShipComponent;
	// 			shipComponents.Add(shipComponent); 
	// 			shipComponent.OnDestroyed += ComponentDestroyed;
				
	// 			foreach (Vector2 v2 in shipComponent.GetVertices())
	// 			{ 
	// 				Vector2 localPositon = ToLocal(v2);
	// 				unpackedVectors.Add(localPositon);
	// 			}
	// 		}
			
	// 	}
	// 	CalculateCentreOfMass(unpackedVectors);
	// 	ConvexPolygonShape2D convexPolygon = new ();
	// 	convexPolygon.SetPointCloud(unpackedVectors.ToArray());
	// 	collider.Polygon = convexPolygon.Points;
		

	// 	cargoManager.cargoCapacity = cargoCapacity;
	// 	maxPowerGenerated = powerManager.GetMaxPowerGenerated();
	// 	thrusterPowerUsageUnderMaxPower = maxPowerGenerated >= thrustPowerNeeded;
	// 	fuel = fuelCapacity; // remove this line later so that fuel doesnt reset when a ship is re-instantiated

	// 	return hasFuelTank && hasGenerator && hasThruster && thrusterPowerUsageUnderMaxPower;
	// }

}

