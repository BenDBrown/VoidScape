using Godot;
using System;

public struct DamageInfo
{
	public int damage { get; set; }

	public DamageInfo(int damage)
	{
		this.damage = damage;
	}
}
