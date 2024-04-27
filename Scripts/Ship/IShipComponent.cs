using Godot;
using System;

public partial interface IShipComponent
{
	public abstract void TakeDamage(DamageInfo damageInfo);

	public abstract void Heal(int healing);

}
