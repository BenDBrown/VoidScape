using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PowerManager : Node
{
	[Signal]
	public delegate void PowerChangedEventHandler(float powerToMaxPowerPercentage); // as the param name implies, this signal returns the percentage of MaxPower that power is

	[Signal]
	public delegate void StallStartedEventHandler(double stallTime);

	[Signal]
	public delegate void StallEndedEventHandler();

	//time for which ship stalls when it goes over power limit, in seconds
	[Export]
	private double stallDuration = 5;

	[Export]
	private Timer stallTimer;

	public bool Stalling {get; private set;} = false;

	public float Efficiency {get; private set;} = 0;

	private List<Generator> generators = new ();

	public float MaxPower {get; private set;} = 0;

	public float Power {get; private set;} = 0;

    public override void _Ready()
    {
        stallTimer.Autostart = false;
		stallTimer.OneShot = true;
		stallTimer.Stop();
		stallTimer.Timeout += StallEnd;
    }

    public override void _Process(double delta)
    {
        if(Stalling) return;
		Power += MaxPower * (float)delta;
		Power = Math.Min(Power, MaxPower);
    }

    /// <summary>
    /// Called when a ship that relies on power tries to consume power.
	/// Will return false if there was not enough power available which will then initiate a stall.
	/// Will also return false if called while stalling.
	/// Fuel usage checking is not done here as this should be managed by FuelManager.
    /// </summary>
	public bool TryUsePower(float powerWanted, out float fuelUsed)
	{
		fuelUsed = 0;
		if (Stalling) return false;
		Power -= powerWanted;
		Power = Math.Max(Power, 0);
		bool enoughPower = Power > 0;
		fuelUsed = powerWanted / Efficiency;
		EmitSignal(SignalName.PowerChanged, GetPowerPercentage());
		if(!enoughPower) StallStart();
		return enoughPower;
	}

	public void AddGenerator(Generator generator) 
	{
		generators.Add(generator); 
		MaxPower = GetMaxPowerGenerated();
		Power = MaxPower;
		CalculateEfficiency();
		generator.OnDestroyed += OnGeneratorDestroyed;
		EmitSignal(SignalName.PowerChanged, GetPowerPercentage());
	}

	private void OnGeneratorDestroyed(ShipComponent shipComponent)
	{
		if(shipComponent is not Generator generator) {GD.PushError("non generator component sent to power manager on destroy event"); return;}
		if(!generators.Contains(generator)) {GD.PushError("destroyed generator was not in power manager dict"); return;}
		generators.Remove(generator);
		MaxPower = GetMaxPowerGenerated();
		CalculateEfficiency();
		Power = Math.Min(MaxPower, Power);
		generator.OnDestroyed -= OnGeneratorDestroyed;
		EmitSignal(SignalName.PowerChanged, GetPowerPercentage());
	}

	private void StallStart()
	{
		stallTimer.Start(stallDuration);
		Stalling = true;
		EmitSignal(SignalName.StallStarted, stallDuration);
	}

	private void StallEnd()
	{
		Stalling = false;
		Power = MaxPower;
		EmitSignal(SignalName.StallEnded);
	}

	private float GetMaxPowerGenerated()
	{
		float maxPowerGenerated = 0;
		foreach (Generator generator in generators){ maxPowerGenerated += generator.maxPowerGenerated; }
		return maxPowerGenerated;
	}

	private float CalculateEfficiency()
	{
		Efficiency = 0;
		foreach(Generator generator in generators) Efficiency += generator.efficiency;
		Efficiency /= generators.Count;
		return Efficiency;
	}

	private float GetPowerPercentage() => (Power / MaxPower) * 100;
}
