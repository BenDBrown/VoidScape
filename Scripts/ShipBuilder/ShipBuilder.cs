using Godot;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class ShipBuilder : Node
{
	private const int MIN_GRID_SIZE = 1;

	[Signal]
	public delegate void ShipBuildAttemptEventHandler(bool success);

	[Export]
	private int gridHeight;

	[Export]
	private int gridWidth;

	[Export]
	private PackedScene gridTile;

	[Export]
	private PackedScene shipScene;

	private Dictionary<Vector2, GridSquare> grid = new();

	public override void _Ready()
	{
		MakeGrid();
		Node buildItems = GetNode("ItemLister");
		buildItems.Connect("added_child", new Callable(this, MethodName.OnBuildItemsChildAdded));
		buildItems.Call("display_items");
	}

	public void BuildShip()
	{
		PlayerShip ship = shipScene.Instantiate() as PlayerShip;
		AddChild(ship);
		foreach (GridSquare square in grid.Values)
		{
			ShipComponent shipComponent = square.shipComponent;
			if (shipComponent != null)
			{
				shipComponent.GetParent().RemoveChild(shipComponent);
				ship.AddChild(shipComponent);
				shipComponent.Position = square.CoordinateToPosition();
			}
		}
		EmitSignal(SignalName.ShipBuildAttempt, ship.TryBuildShip());
	}

	public Godot.Collections.Dictionary GetDict()
	{
		Godot.Collections.Dictionary dict = new();
		foreach (GridSquare square in grid.Values)
		{
			if (square.shipComponent != null)
			{
				dict[square.Coordinate] = square.shipComponent;
			}
		}
		return dict;
	}

	private void MakeGrid()
	{
		grid = new();
		gridHeight = Mathf.Max(gridHeight, MIN_GRID_SIZE);
		gridWidth = Mathf.Max(gridWidth, MIN_GRID_SIZE);
		for (int x = 0; x < gridWidth; x++)
		{
			for (int y = 0; y < gridHeight; y++)
			{
				GridSquare square = gridTile.Instantiate() as GridSquare;
				AddChild(square);
				square.Coordinate = new Vector2(x, y);
				grid.Add(square.Coordinate, square);
				square.Position = square.CoordinateToPosition();
				square.ShipComponentChanged += UpdatePlacementValidity;
			}
		}
	}

	private void UpdatePlacementValidity(object sender)
	{
        bool noComponents = true;
		foreach(GridSquare square in grid.Values)
		{
            if(square.shipComponent != null) { noComponents = false; }
			square.SetValidity(false);
		}
        if(noComponents) { foreach(GridSquare square in grid.Values) { square.SetValidity(true); } }
		foreach(Vector2 coord in grid.Keys)
		{
			if(grid[coord].shipComponent == null) { continue; }
			grid[coord].SetValidity(true);
			Vector2 vRight = new(coord.X + 1, coord.Y);
			Vector2 vLeft = new(coord.X - 1, coord.Y);
			Vector2 vUp = new(coord.X, coord.Y - 1);
			Vector2 vDown = new(coord.X, coord.Y + 1);
			if(grid.ContainsKey(vRight))
			{
				if(grid[coord].shipComponent.RightAttachable) { grid[vRight].SetValidity(true); }
			}
			if(grid.ContainsKey(vLeft))
			{
				if(grid[coord].shipComponent.LeftAttachable) { grid[vLeft].SetValidity(true); }
			}
			if(grid.ContainsKey(vUp))
			{
				if(grid[coord].shipComponent.TopAttachable) { grid[vUp].SetValidity(true); }
			}
			if(grid.ContainsKey(vDown))
			{
				if(grid[coord].shipComponent.BottomAttachable) { grid[vDown].SetValidity(true); }
			}
		}
	}

	private void OnBuildItemsChildAdded(Node2D child)
	{
		AddChild(child);
	}

}
