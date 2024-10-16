using Godot;
using System;
using System.Collections.Generic;

public partial class GunManager
{
	private GunGroup selectedGroup = null;
	private List<GunGroup> gunGroups = new();

	public GunManager() { }

	public void AddGun(Gun gun)
	{
		bool gunAdded = false;
		foreach (GunGroup gg in gunGroups)
		{
			if(gg.type == gun.type) 
			{
				gg.AddGun(gun);
				gunAdded = true;
				break;
			}
		}
		if(!gunAdded) { gunGroups.Add(new(gun)); }
		if(selectedGroup == null)
		{
			selectedGroup = gunGroups[0];
		}
	}

	public void StartShooting() => selectedGroup.StartShooting();

	public void StopShooting() => selectedGroup.StopShooting();
}
