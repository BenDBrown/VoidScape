using Godot;
using System;
using System.Collections.Generic;

public partial class CargoManager : Node
{
	Dictionary<Cargo, int> cargoDict = new ();

	public int cargoCapacity {get; set;}

	public int GetCargoCount()
	{
		int cargoCount = 0;
		foreach (Cargo cargo in cargoDict.Keys)
		{
			cargoCount += cargoDict[cargo];
		}
		return cargoCount;
	}

	public bool TryAddCargo(Cargo cargo, int quantity, out int cargoAdded)
	{
		if(GetCargoCount() + quantity <= cargoCapacity)
		{
			if(cargoDict.ContainsKey(cargo)) { cargoDict[cargo] += quantity; }
			else { cargoDict.Add(cargo, quantity); }
			cargoAdded = quantity;
			return true;
		}
		else
		{
			cargoAdded = cargoCapacity - GetCargoCount();
			if(cargoDict.ContainsKey(cargo)) { cargoDict[cargo] += cargoAdded; }
			else { cargoDict.Add(cargo, cargoAdded); }
			return false;
		}
	}

	public bool TryTakeCargo(Cargo cargo, int quantity)
	{
		if(cargoDict.ContainsKey(cargo) && cargoDict[cargo] >= quantity) { return true; }
		return false;
	}

}
