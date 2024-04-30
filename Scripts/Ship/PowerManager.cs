using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PowerManager : Node
{
	private List<Generator> generators = new List<Generator>();

	public int GetMaxPowerGenerated()
	{
		int maxPowerGenerated = 0;
		foreach (Generator generator in generators){ maxPowerGenerated += generator.maxPowerGenerated; }
		return maxPowerGenerated;
	}

	public void TryUsePower(int powerWanted, int fuelAvailable, out int fuelUsed, out bool enoughPower, out bool enoughFuel)
	{
		fuelUsed = 0;
		enoughPower = false;
		enoughFuel = false;
		for(int i = 0; i < generators.Count; i++)
		{
			if(powerWanted > generators[i].maxPowerGenerated)
			{
				fuelUsed += Mathf.RoundToInt(generators[i].maxPowerGenerated / generators[i].efficiency);
				powerWanted -= generators[i].maxPowerGenerated;
			}
			else
			{
				fuelUsed += Mathf.RoundToInt(powerWanted / generators[i].efficiency);
				enoughPower = true;
				powerWanted = 0;
			}
		}
		if(fuelAvailable >= fuelUsed) { enoughFuel = true; }
	}

	public void AddGenerator(Generator generator) {generators.Add(generator); UpdateGeneratorOrder(); }

	public void RemoveGenerator(Generator generator) {generators.Remove(generator); }

	private void UpdateGeneratorOrder() { generators = generators.OrderByDescending(Generator => Generator.efficiency).ToList(); }
}
