using Godot;
using System;

public partial interface IShipComponent
{
	public abstract void TakeDamage(int damage);

	public abstract void Heal(int healing);

}
