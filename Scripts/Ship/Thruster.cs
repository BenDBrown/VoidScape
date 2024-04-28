using Godot;
using System;

public partial class Thruster : Node2D, IShipComponent, IPowerable
{
	[Export]
    private DefenseInfo defenseInfo;
	
	[Export]
	private float thrust;

	[Export]
	private int powerdraw;

    public DefenseInfo GetDefenseInfo() => defenseInfo;

    public int GetPowerDraw() => powerdraw;

	public float GetThrust() => thrust;

}
