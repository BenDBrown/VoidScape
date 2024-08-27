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

    private List<GridSquare> grid = new()
    ;
    public override void _Ready()
    {
        MakeGrid();
    }

    public void BuildShip()
    {
        PlayerShip ship = shipScene.Instantiate() as PlayerShip;
        AddChild(ship);
        Vector2 cockpitOffset = grid.FirstOrDefault(s => s.shipComponent is Cockpit).CoordinateToPosition();
        foreach (GridSquare square in grid)
        {
            ShipComponent shipComponent = square.shipComponent;
            if (shipComponent != null)
            {
                shipComponent.GetParent().RemoveChild(shipComponent);
                ship.AddChild(shipComponent);
                shipComponent.Position = square.CoordinateToPosition() - cockpitOffset;
            }
        }
        EmitSignal(SignalName.ShipBuildAttempt, ship.TryBuildShip());
    }

    public Godot.Collections.Dictionary GetDict()
    {
        Godot.Collections.Dictionary dict = new();
        Vector2 cockpitOffset = grid.FirstOrDefault(s => s.shipComponent is Cockpit).Coordinate;
        foreach (GridSquare square in grid)
        {
            if (square.shipComponent != null)
            {
                dict[square.Coordinate - cockpitOffset] = square.shipComponent;
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
                grid.Add(square);
                square.Position = square.CoordinateToPosition();
            }
        }
    }
}
