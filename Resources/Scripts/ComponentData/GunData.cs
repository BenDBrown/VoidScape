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
	public AudioStream ShootingAudio;
	[Export]
	public float shootingAudiofromPosition = 0;
	[Export]
	public float shootingAudioEndPosition = -1; // -1 means until the end of the track

	[ExportCategory("Bullet Data")]

	[Export]
	public int BulletSpeed;

	[Export]
	public Vector2 BulletSpawnPoint;

	[Export]
	public PackedScene BulletPrefab;
}
