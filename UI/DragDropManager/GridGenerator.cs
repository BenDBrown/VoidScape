using Godot;
using System;

public partial class GridGenerator : GridContainer
{

	[Export]
	private int gridColumns = 10;

	[Export]
	private int gridRows;

	[Export]
	private Vector2 cellSize = new Vector2(32, 32);

	public Panel[,] gridCells;


	public override void _Ready()
	{
		Columns = gridColumns;
		gridCells = new Panel[gridRows, gridColumns];
		GenerateGrid();
	}


	private void GenerateGrid()
	{

		for (int row = 0; row < gridRows; row++)
		{
			for (int col = 0; col < gridColumns; col++)
			{
				Panel cell = new Panel
				{
					Name = $"Cell_{row}_{col}",
					CustomMinimumSize = cellSize,
					ClipContents = true,
					Modulate = new Color(1, 1, 1, 1)
				};

				cell.Position = new Vector2(col * cellSize.X, row * cellSize.Y);

				AddChild(cell);
				gridCells[row, col] = cell;
			}
		}
	}

	public Panel GetCellAt(int row, int col)
	{
		if (row >= 0 && row < gridRows && col >= 0 && col < gridColumns)
		{
			return gridCells[row, col];
		}
		return null;
	}


}
