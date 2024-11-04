using Godot;
using System;

[GlobalClass]
public partial class GunData : ShipComponentData
{

	[Export]
	private float fireRatePerSecond;
	[Export]
	private int damage;

	[Export]
	private GunType gunType;

	[ExportCategory("Bullet Data")]

	[Export]
	private int bulletSpeed;

	[Export]
	private int bulletSpawnOffset;

	[Export]
	private PackedScene bulletPrefab;

	public override void SetUp(ShipComponent component)
	{
		base.SetUp(component);
		if (component is Gun gun)
		{
			//Set gun stuff here
		}
	}
}