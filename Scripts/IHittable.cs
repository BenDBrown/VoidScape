using Godot;
using System;

public interface IHittable
{
	void TakeDamage(DamageInfo damageInfo);
}
