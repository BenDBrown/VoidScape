using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GridGenerator : Control
{

	[Export]
	private int gridColumns = 10;

	[Export]
	private int gridRows;

	[Export]
	private Vector2 cellSize = new Vector2(32, 32);

	[Export]
	public Texture2D FreeCell { get; set; }
	[Export]
	public Texture2D ValidCell { get; set; }
	[Export]
	public Texture2D InvalidCell { get; set; }

	public GridTile GridCell { get; private set; }

	public Dictionary<Vector2I, TextureRect> GridCells { get; private set; }

	public List<TextureRect> GenerateGrid()
	{
		GridCells = new();
		for (int row = 0; row < gridRows; row++)
		{
			for (int col = 0; col < gridColumns; col++)
			{
				GridCell = new GridTile
				{
					Name = $"Cell_{row}_{col}",
					CustomMinimumSize = cellSize,
					ClipContents = true,
					Texture = FreeCell
				};

				GridCell.Position = new Vector2(col * cellSize.X, row * cellSize.Y);

				AddChild(GridCell);
				GridCells.Add(new(row, col), GridCell);
			}
		}
		return GridCells.Values.ToList();
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
