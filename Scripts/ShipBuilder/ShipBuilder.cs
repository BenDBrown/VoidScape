using Godot;
using System;
using System.Collections.Generic;

public partial class ShipBuilder : Node
{
	private const int DICT_POS_RATIO = 32;

	[Signal]
    public delegate void ShipBuildAttemptEventHandler(bool success);

	[Export]
	private int gridHeight;
	
	[Export]
	private int gridWidth;

	[Export]
	PackedScene gridTile;

	[Export]
	PackedScene shipScene;

	private readonly int minGridSize = 1;

	private Dictionary<Vector2, GridSquare> gridDict = new();

    public override void _Ready()
    {
        if(gridHeight < minGridSize) { gridHeight = minGridSize; }
		if(gridWidth < minGridSize) { gridWidth = minGridSize; }
		MakeGrid();
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("confirm")){BuildShip();}
    }

    public void BuildShip()
	{
		PlayerShip ship = shipScene.Instantiate() as PlayerShip;
		AddChild(ship);
		foreach(Vector2 pos in gridDict.Keys)
		{
			ShipComponent shipComponent = gridDict[pos].shipComponent;
			if(shipComponent != null)
			{
				shipComponent.GetParent().RemoveChild(shipComponent);
				ship.AddChild(shipComponent);
				shipComponent.Position = ConvertDictPosToLocalPos(pos);
			}
		}
		EmitSignal(SignalName.ShipBuildAttempt, ship.TryBuildShip());
	}

	private void MakeGrid()
	{
		for(int x = 0; x < gridWidth; x++)
		{
			for(int y = 0; y < gridHeight; y++)
			{
				GridSquare newTile = gridTile.Instantiate() as GridSquare;
				AddChild(newTile);
				Vector2 newDictPos = new Vector2(x, y);
				gridDict.Add(newDictPos, newTile);
				newTile.Position = ConvertDictPosToLocalPos(newDictPos);
			}
		}
	}

	private Vector2 ConvertDictPosToLocalPos(Vector2 v2)
	{
		return new Vector2(v2.X * DICT_POS_RATIO, v2.Y * DICT_POS_RATIO);
	}
}
