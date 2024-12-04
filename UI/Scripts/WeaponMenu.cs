using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class WeaponMenu : Control
{
	[Export]
	private WeaponMenuOption[] menuOptions;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Setting up singals
		Game.Instance.PlayerShip.WeaponMenuToggled += ToggleMenu;
		Game.Instance.PlayerShip.GunCycleChanged += CycleThroughMenu;
	}


	private void ToggleMenu(bool isOpen){

		// Create the logic to when closing don't create a menu

		this.Visible = isOpen;
		if(isOpen){
			CreateMenu();
		}
	}

	private void CycleThroughMenu(int cycleNum){
		CreateMenu();
	}

	private void CreateMenu(){
		List<GunType> gunTypes = Game.Instance.PlayerShip.GetAvailableGunTypes();
		int curSelectedIndex = Game.Instance.PlayerShip.GetActiveWeaponIndex(); // Used to set the right option in the UI

		for(int i = 0; i < menuOptions.Length ; i++){
			WeaponBorderState newBorder = WeaponBorderState.UNAVAILABLE; // Default State: no gun type available to select

			if(i <= (gunTypes.Count - 1)){

				// Attaching the right icon of the gun 
				AttachIcon(gunTypes[i], i); // Todo: Setting the texture everytime the menu opens is bad practice. Better to do it once and only check for it when a gun is changed when rebuilding the ship. For now okay.

				newBorder = WeaponBorderState.UNSELECTED;

				if(i == curSelectedIndex){
					newBorder = WeaponBorderState.SELECTED;
				}
			}
			menuOptions[i].UpdateBorder(newBorder);
		}
	}

	private void AttachIcon(GunType gunType, int indexOfOption){
		Texture2D iconForGunType = Game.Instance.PlayerShip.GetGunTypeIcon(gunType);
		menuOptions[indexOfOption].UpdateWeaponIcon(iconForGunType);
	}
}
