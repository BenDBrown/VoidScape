using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GridGenerator : Control
{
	[Export]
	public Texture2D FreeCell { get; private set; }
	[Export]
	public Texture2D ValidCell { get; private set; }
	[Export]
	public Texture2D InvalidCell { get; private set; }

	[Export]
	private int gridColumns = 10;

	[Export]
	private int gridRows;

	[Export]
	private Vector2 cellSize = new Vector2(32, 32);

	[Export]
	private ShaderMaterial shaderMat;



	public Dictionary<Vector2I, GridTile> GridCells { get; private set; }

	public GridTile[] GenerateGrid()
	{
		GridCells = new();
		for (int row = 0; row < gridRows; row++)
		{
			for (int col = 0; col < gridColumns; col++)
			{
				GridTile gridCell = new GridTile
				{
					Name = $"Cell_{row}_{col}",
					CustomMinimumSize = cellSize,
					ClipContents = true,
					Texture = FreeCell,
					Material = shaderMat
				};

				gridCell.Position = new Vector2(col * cellSize.X, row * cellSize.Y);

				AddChild(gridCell);
				GridCells.Add(new(col, row), gridCell);
			}
		}
		return GridCells.Values.ToArray();
	}

	public void LoadShip()
	{
		var ship = Game.Instance.PlayerShip;
		var parts = ship.ShipComponents;
	}

	public Vector2I GetCellAt(TextureRect rect)
	{
		foreach (Vector2I gridPos in GridCells.Keys)
		{
			if (GridCells[gridPos] == rect) return gridPos;
		}
		return new(-1, -1);
	}

	public void ChangeCellText(TextureRect rect, Texture2D texture2D) => rect.Texture = texture2D;


}
