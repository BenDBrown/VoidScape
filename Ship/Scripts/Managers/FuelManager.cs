using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FuelManager
{
	public delegate void NoFuelEventHandler();

	public event NoFuelEventHandler NoFuel;

	public delegate void AllFuelTanksDestroyedEventHandler();

	public event AllFuelTanksDestroyedEventHandler AllFuelTanksDestroyed;

    public float FuelCapacity {get; private set;}

	public float Fuel {get; private set;}

	public FuelManager() { }

	public void AddFuel(float fuel)
	{
		Fuel += fuel;
		Fuel = Math.Min(Fuel, FuelCapacity);
	}

	public void UseFuel(float fuel)
	{
		Fuel -= fuel;
		if(Fuel <= 0)
		{
			Fuel = 0;
			NoFuel?.Invoke();
		}
	}

	public void AddFuelTank(FuelTank fuelTank)
	{
		FuelCapacity += fuelTank.fuelCapacity;
		fuelTank.OnDestroyed += OnFuelTankDestroyed;
	}

	public void OnFuelTankDestroyed(ShipComponent component)
	{
		if(!(component is FuelTank fuelTank)) { GD.PushError("Non fuel tank ship component passed to fuel manager on destroy"); return; }
		FuelCapacity -= fuelTank.fuelCapacity;
		if(FuelCapacity <= 0) { AllFuelTanksDestroyed?.Invoke(); }
		if(FuelCapacity >= Fuel) { return; }
		Fuel = FuelCapacity;
		fuelTank.OnDestroyed -= OnFuelTankDestroyed;
	}

}
