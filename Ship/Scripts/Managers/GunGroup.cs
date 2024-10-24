using System;
using Godot;
using System.Collections.Generic;

public class GunGroup : IPowerable
{
	public delegate void GunGroupDestroyedEventHandler(GunGroup gunGroup);
	
	public event GunGroupDestroyedEventHandler GroupDestroyed;

	public delegate void ToggleShootingEventHandler();

	public event ToggleShootingEventHandler StartedShooting;

	public event ToggleShootingEventHandler StoppedShooting;

	public GunType type {get; private set;}

	public List<Gun> guns = new();

	public int PowerDraw { get; private set; }	= 0;

	public GunGroup(Gun gun) 
	{ 
		type = gun.type;
		AddGun(gun);
	}

	public void StartShooting() => StartedShooting?.Invoke();

	public void StopShooting() => StoppedShooting?.Invoke();

	public void AddGun(Gun gun)
	{ 
		guns.Add(gun);
		PowerDraw += gun.GetPowerDraw();
		StartedShooting += gun.StartShooting;
		StoppedShooting += gun.StopShooting;
		gun.OnDestroyed += OnGunDestroyed; 
	} 

	public void RemoveGun(Gun gun, out bool groupEmpty) 
	{ 
		guns.Remove(gun);
		PowerDraw -= gun.GetPowerDraw();
		StartedShooting -= gun.StartShooting;
		StoppedShooting -= gun.StopShooting;
		groupEmpty = guns.Count <= 0;
		gun.OnDestroyed -= OnGunDestroyed; 
	}

	public int GetPowerDraw() => PowerDraw;

	private void OnGunDestroyed(ShipComponent shipComponent)
	{
		if(!(shipComponent is Gun gun)) { GD.PushError("Non gun ship component passed to gun group on destroy"); return; }
		RemoveGun(gun, out bool groupEmpty);
		if(groupEmpty) { GroupDestroyed?.Invoke(this); }
	}

}
