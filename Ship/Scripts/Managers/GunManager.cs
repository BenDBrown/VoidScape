using Godot;
using System;
using System.Collections.Generic;

public partial class GunManager : IPowerable
{
	public int PowerDraw => GetPowerDraw();
	private GunGroup selectedGroup = null;
	private List<GunGroup> gunGroups = new();
	private bool shooting = false;

	public GunManager() { }

	public void AddGun(Gun gun)
	{
		bool gunAdded = false;
		foreach (GunGroup gg in gunGroups)
		{
			if (gg.type == gun.type)
			{
				gg.AddGun(gun);
				gunAdded = true;
				break;
			}
		}
		if (!gunAdded) { gunGroups.Add(new(gun)); }
		if (selectedGroup == null)
		{
			selectedGroup = gunGroups[0];
		}
	}

	public int GetPowerDraw()
	{
		if(shooting) return selectedGroup.PowerDraw;
		return 0;
	}

	public void StartShooting() 
	{
		selectedGroup?.StartShooting();
		shooting = true;
	}

	public void StopShooting() 
	{
		selectedGroup?.StopShooting();
		shooting = false;
	}

	public void CycleGunGroupUp() => CycleGunGroup(1);

	public void CycleGunGroupDown() => CycleGunGroup(-1);

	private void CycleGunGroup(int cycleNum)
	{
		if(!gunGroups.Contains(selectedGroup)) {GD.PushError("had a gun group selected that was not stored in GunManager"); return;}
		int currentIndex = gunGroups.IndexOf(selectedGroup);
		if(currentIndex + cycleNum >= gunGroups.Count) // logic to make selection loop at end of list
		{
			selectedGroup = gunGroups[0];
			return;
		}
		else if (currentIndex + cycleNum < 0)
		{
			selectedGroup = gunGroups[^1];
		}
		selectedGroup = gunGroups[currentIndex + 1];
	}
}
