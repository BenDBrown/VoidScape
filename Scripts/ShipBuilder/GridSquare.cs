using Godot;
using System;

[GlobalClass]
public partial class GridSquare : StaticBody2D
{
	[Export]
	public ShipComponent shipComponent{ get; set; }
}
