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
			if(gg.type == gun.type){
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

	public int GetSelectedWeaponIndex(){
		return gunGroups.IndexOf(selectedGroup);
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

	public List<GunType> GetGunGroupTypes(){
		List<GunType> gunTypes = new();
		foreach(GunGroup gg in gunGroups){
			gunTypes.Add(gg.type);
		}

		return gunTypes;
	}

	public Texture2D GetGunTypeIcon(GunType type){

			foreach(GunGroup gg in gunGroups){
				if(gg.type == type){
					return gg.gunTypeIcon;
				}
			}
			return new();
	}

	/// <summary>
	/// Cycles between the available weapons. 
	/// </summary>
	/// <param name="cycleNum">The direction of the cycle. A 1 means to go up in the list and -1 go down in the list.</param>
	public void CycleGunGroup(int cycleNum)
	{
		if(!gunGroups.Contains(selectedGroup)) {GD.PushError("had a gun group selected that was not stored in GunManager"); return;}
		int currentIndex = gunGroups.IndexOf(selectedGroup);

		int newIndex = currentIndex - cycleNum;
		if(newIndex < 0){
			newIndex = gunGroups.Count - 1; // Last index of gun groups

		}
		else if(newIndex >= gunGroups.Count){ // Loop to beginning
			newIndex = 0;
		}
		else{
			selectedGroup = gunGroups[newIndex];
		}

		selectedGroup = gunGroups[newIndex];
	}
}
