using Godot;
using System;

[GlobalClass]
public partial class GunData : ShipComponentData
{

	[Export]
	public double BulletsPerSecond;
	[Export]
	public int Attack = 100;
	[Export]
	public int Powerdraw = 10;
	[Export]
	public GunType type;

	[ExportCategory("Bullet Data")]

	[Export]
	public int BulletSpeed;

	[Export]
	public Vector2 BulletSpawnPoint;

	[Export]
	public PackedScene BulletPrefab;
}
