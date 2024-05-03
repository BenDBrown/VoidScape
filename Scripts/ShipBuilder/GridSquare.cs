using Godot;
using System;

public partial class GridSquare : StaticBody2D
{
	[Export]
	public ShipComponent shipComponent{ get; set; }
}
