using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CargoManager
{
	public delegate void CargoLostEventHandler(Dictionary<Cargo, int> lostCargoDict);

	public event CargoLostEventHandler CargoLost;

	Dictionary<Cargo, int> cargoDict = new ();
	public int MaxCargoCapacity {get; private set;} = 0;
	public int CargoAmount {get; private set;} = 0;
	public int CurrentCargoCapacity => MaxCargoCapacity - CargoAmount;

	public CargoManager() { }

	public bool TryAddCargo(Cargo cargo, int quantity, out int cargoAdded)
	{
		if(CargoAmount + quantity <= MaxCargoCapacity)
		{
			if(cargoDict.ContainsKey(cargo)) { cargoDict[cargo] += quantity; }
			else { cargoDict.Add(cargo, quantity); }
			cargoAdded = quantity;
			UpdateCargoAmount();
			return true;
		}
		cargoAdded = CurrentCargoCapacity;
		if(cargoDict.ContainsKey(cargo)) { cargoDict[cargo] += cargoAdded; }
		else { cargoDict.Add(cargo, cargoAdded); }
		UpdateCargoAmount();
		return false;
	}

	public bool TryTakeCargo(Cargo cargo, int quantity)
	{
		if(!(cargoDict.ContainsKey(cargo) && cargoDict[cargo] >= quantity)) { return false; }
		cargoDict[cargo] -= quantity;
		if(cargoDict[cargo] <= 0) { cargoDict.Remove(cargo); }
		UpdateCargoAmount();
		return true;
	}

	public void AddHull(Hull hull)
	{
		MaxCargoCapacity += hull.cargoCapacity;
		hull.OnDestroyed += OnHullDestroyed;
	}

	public void OnHullDestroyed(ShipComponent component)
	{
		if(!(component is Hull hull)) { GD.PushError("Non hull ship component passed to cargo manager on destroy"); return; }
		MaxCargoCapacity -= hull.cargoCapacity;
		hull.OnDestroyed -= OnHullDestroyed;
		if(MaxCargoCapacity >= CargoAmount) { return; }
		List<Cargo> cargos = cargoDict.Keys.ToList();
		int count = cargos.Count;
		Dictionary<Cargo, int> lostCargoDict = new();
		Random rng = new();
		for(int cargoToBeLost = CargoAmount - MaxCargoCapacity; cargoToBeLost > 0; cargoToBeLost--)
		{
			Cargo cargo = cargos[rng.Next(count)];
			cargoDict[cargo]--;
			if(lostCargoDict.ContainsKey(cargo)) { lostCargoDict[cargo]++; }
			else { lostCargoDict.Add(cargo, 1); }
			if(cargoDict[cargo] <= 0) { cargoDict.Remove(cargo); }
		}
		UpdateCargoAmount();
		CargoLost?.Invoke(lostCargoDict);
	}

	private void UpdateCargoAmount()
	{
		CargoAmount = 0;
		foreach (Cargo cargo in cargoDict.Keys)
		{
			CargoAmount += cargoDict[cargo];
		}
	}

}
