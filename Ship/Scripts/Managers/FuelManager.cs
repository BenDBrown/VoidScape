using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FuelManager
{
	public delegate void NoFuelEventHandler();
	public event NoFuelEventHandler NoFuel;

	public delegate void FueldChangedEventHandler(float fuelToMaxFuelPercentage);
	public event FueldChangedEventHandler FuelChanged;

    public float FuelCapacity {get; private set;} = 0;

	public float Fuel {get; private set;} = 0;

	public FuelManager() { }

	public void AddFuel(float fuel)
	{
		fuel = Math.Max(fuel, 0);
		Fuel += fuel;
		Fuel = Math.Min(Fuel, FuelCapacity);
		FuelChanged?.Invoke(GetFuelPercentage());
	}

	public void UseFuel(float fuel)
	{
		fuel = Math.Max(fuel, 0);
		Fuel -= fuel;
		if(Fuel < 0)
		{
			Fuel = 0;
			NoFuel?.Invoke();
		}
		FuelChanged?.Invoke(GetFuelPercentage());
	}

	public void AddFuelTank(FuelTank fuelTank)
	{
		FuelCapacity += fuelTank.FuelCapacity;
		fuelTank.OnDestroyed += OnFuelTankDestroyed;
	}

	public void OnFuelTankDestroyed(ShipComponent component)
	{
		if(!(component is FuelTank fuelTank)) { GD.PushError("Non fuel tank ship component passed to fuel manager on destroy"); return; }
		FuelCapacity -= fuelTank.FuelCapacity;
		fuelTank.OnDestroyed -= OnFuelTankDestroyed;
		if(FuelCapacity >= Fuel) { return; }
		Fuel = FuelCapacity;
	}

	private float GetFuelPercentage() => (Fuel / FuelCapacity) * 100;

}
