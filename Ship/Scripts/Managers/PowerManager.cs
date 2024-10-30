using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PowerManager : Node
{
	//in seconds
	private const int POWER_TICK_RATE = 1;

	private const int STALL_TIMER = 5;

	public bool stalling {get; set;}

	[Export]
	private Timer timer;

	private Dictionary<Generator, float> generatorPowerUsedDict = new ();

	private float maxPower = 0;

	private float power = 0;

    public override void _Ready()
    {
        timer.Start(POWER_TICK_RATE);
		timer.Timeout += PowerTick;
    }

    public float GetMaxPowerGenerated()
	{
		float maxPowerGenerated = 0;
		foreach (Generator generator in generatorPowerUsedDict.Keys){ maxPowerGenerated += generator.maxPowerGenerated; }
		return maxPowerGenerated;
	}

	public bool TryUsePower(float powerWanted, float fuelAvailable, out float fuelUsed, out bool hasEnoughFuel)
	{
		fuelUsed = 0;
		bool enoughPower = false;
		hasEnoughFuel = false;
		float powerGenerated = 0;
		if(powerWanted > power) { hasEnoughFuel = true; return enoughPower && hasEnoughFuel; }
		foreach(Generator generator in GetOrderedGenerators())
		{
			if(generator.maxPowerGenerated >= generatorPowerUsedDict[generator]) 
			{
				float maxPowerChunk = generator.maxPowerGenerated - generatorPowerUsedDict[generator];
				float powerChunk = powerWanted - powerGenerated;
				if(powerChunk > maxPowerChunk) { powerChunk = maxPowerChunk; }
				generatorPowerUsedDict[generator] += powerChunk;
				powerGenerated += powerChunk;
				fuelUsed += powerChunk / generator.efficiency;
				power -= powerChunk;
			}
			if(powerGenerated >= powerWanted)
			{
				if(fuelAvailable >= fuelUsed) {hasEnoughFuel = true;}
				enoughPower = true;
				return enoughPower && hasEnoughFuel;
			}
		}
		if(fuelAvailable >= fuelUsed)
		{
			GD.Print("max power: " + maxPower + ", power: " + power);
			hasEnoughFuel = true;
			stalling = true;
		}
		return enoughPower && hasEnoughFuel;
	}

	public void AddGenerator(Generator generator) 
	{
		generatorPowerUsedDict.Add(generator, 0); 
		maxPower = GetMaxPowerGenerated();
		power = maxPower;
		GetOrderedGenerators(); 
	}

	public void RemoveGenerator(Generator generator) {generatorPowerUsedDict.Remove(generator); }

	private void PowerTick()
	{
		if(stalling) { timer.Start(STALL_TIMER); stalling = false; return; }
		power = maxPower;
		foreach(Generator generator in generatorPowerUsedDict.Keys){ generatorPowerUsedDict[generator] = 0; }
		timer.Start(POWER_TICK_RATE);
	}

	private List<Generator> GetOrderedGenerators() { return generatorPowerUsedDict.Keys.OrderByDescending(Generator => Generator.efficiency).ToList(); }
}
