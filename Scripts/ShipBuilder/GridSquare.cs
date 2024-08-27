using Godot;
using System;

[GlobalClass]
public partial class GridSquare : StaticBody2D
{
	private const int COORDINATE_RATIO = 32;
	[Export]
	public ShipComponent shipComponent { get; set; }
	public Vector2 Coordinate { get; set; }
	public Vector2 CoordinateToPosition() => new Vector2(Coordinate.X * COORDINATE_RATIO, Coordinate.Y * COORDINATE_RATIO);
}