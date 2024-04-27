using Godot;
using System;

public interface IShipComponent
{
	DefenseInfo GetDefenseInfo();

	public delegate void DestroyedEventHandler(IShipComponent shipComponent);
}
