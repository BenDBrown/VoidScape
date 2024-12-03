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
	private Texture2D freeCell;

	public Dictionary<Vector2I, TextureRect> gridCells;

	public List<TextureRect> GenerateGrid()
	{
		gridCells = new();
		for (int row = 0; row < gridRows; row++)
		{
			for (int col = 0; col < gridColumns; col++)
			{
				TextureRect cell = new TextureRect
				{
					Name = $"Cell_{row}_{col}",
					CustomMinimumSize = cellSize,
					ClipContents = true,
					Texture = freeCell
				};

				cell.Position = new Vector2(col * cellSize.X, row * cellSize.Y);

				AddChild(cell);
				gridCells.Add(new(row, col), cell);
			}
		}
		return gridCells.Values.ToList();
	}

	public Vector2I GetCellAt(TextureRect rect)
	{
		foreach (Vector2I gridPos in gridCells.Keys)
		{
			if (gridCells[gridPos] == rect) return gridPos;
		}
		return new(-1, -1);
	}

	public void ChangeCellText(TextureRect rect, Texture2D texture2D)
	{
		rect.Texture = texture2D;

	}


}
