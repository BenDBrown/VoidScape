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
	[Export]
	public Texture2D GunTypeIcon;

	[ExportCategory("Shooting Audio Data")]
	[Export]
	public AudioStream ShootSFX;
	[Export]
	public float ShootingAudioStartTime = 0;

	/// <summary>
	/// 
	/// A value of -1 is the same as: "until the end of the track"
	/// </summary>
	[Export]
	public float ShootingAudioEndTime = -1;

	[ExportCategory("Bullet Data")]

	[Export]
	public int BulletSpeed;

	[Export]
	public Vector2 BulletSpawnPoint;

	[Export]
	public PackedScene BulletPrefab;
}
