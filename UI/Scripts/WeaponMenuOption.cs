using Godot;
using System;
using System.IO;

public partial class WeaponMenuOption : Control
{

	[Export]
	private TextureRect weaponTypeIcon;

	[Export]
	private TextureRect optionBorder;

	[Export]
	private AtlasTexture selected, unselected, unavailable;

	public override void _Ready()
	{
	}

	public void UpdateWeaponIcon(Texture2D texture){
		weaponTypeIcon.Texture = texture;
	}

	public void UpdateBorder(WeaponBorderState newstate){
		switch(newstate){
			case WeaponBorderState.UNSELECTED:
				optionBorder.Texture = unselected;
				break;
			case WeaponBorderState.SELECTED:
				optionBorder.Texture = selected;
				break;
			case WeaponBorderState.UNAVAILABLE:
				optionBorder.Texture = unavailable;
				break;
		}
	}
}

public enum WeaponBorderState {
	UNSELECTED = 0,
	SELECTED = 1,
	UNAVAILABLE = 2
}
