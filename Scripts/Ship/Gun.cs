using Godot;
using System;

public partial class Gun : Node2D, IShipComponent, IPowerable
{
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

    public DefenseInfo GetDefenseInfo()
    {
        throw new NotImplementedException();
    }

    public int GetPowerDraw()
    {
        throw new NotImplementedException();
    }
}
