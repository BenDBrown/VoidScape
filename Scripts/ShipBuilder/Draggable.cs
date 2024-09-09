using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Draggable : Node2D
{
	private static Draggable selected;
	[Export]
	public ShipComponent shipComponent { get; private set; }

	private bool isInDroppable = false;

	private bool draggable = false;
	private bool isSelectable = false;

	private List<GridSquare> gridSquares = new();

	private Vector2 mouseOffset;

	private Vector2 initialPosition;
	private string clickActionName = "click";

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(clickActionName))
		{
			if (selected == null && isSelectable)
			{
				Select();
			}
		}
		else if (Input.IsActionJustReleased(clickActionName))
		{
			if (selected == this)
			{
				Deselect();
			}
		}
		if (draggable)
		{
			GlobalPosition = GetGlobalMousePosition() - mouseOffset;
		}
	}

	public void Select()
	{
		draggable = true;
		selected = this;
		initialPosition = GlobalPosition;
		mouseOffset = GetGlobalMousePosition() - GlobalPosition;
	}

	public void Deselect()
	{
		selected = null;
		draggable = false;
		Tween tween = GetTree().CreateTween();
		if (isInDroppable)
		{
			GridSquare gridSquare = GetNearestGridSquare();
			tween.TweenProperty(this, "position", gridSquare.Position, 0.2f).SetEase(Tween.EaseType.Out);
			gridSquare.shipComponent = shipComponent;
		}
		else
		{
			tween.TweenProperty(this, "global_position", initialPosition, 0.2f).SetEase(Tween.EaseType.Out);
		}
	}

	public void MouseEnter()
	{
		isSelectable = true;
		Scale = new Vector2(1.05f, 1.05f);
	}

	public void MouseExit()
	{
		isSelectable = false;
		Scale = new Vector2(1, 1);
	}

	public void EnteredSnappable(Node2D snappable)
	{
		if (snappable.IsInGroup("snappable"))
		{
			isInDroppable = true;
			gridSquares.Add(snappable as GridSquare);
		}
	}

	public void ExitedSnappable(Node2D snappable)
	{
		if (!(snappable is GridSquare square)) { return; }
		if (gridSquares.Contains(square)) { gridSquares.Remove(square); }
		if (square.shipComponent == shipComponent) { square.shipComponent = null; }
		if (isInDroppable) { return; }
		isInDroppable = false;
	}

	public void SetShipComponent(ShipComponent shipComponent)
	{
		AddChild(shipComponent);
		shipComponent.Position = Position;
		this.shipComponent = shipComponent;
	}

	private GridSquare GetNearestGridSquare()
	{
		float distance = float.MaxValue;
		GridSquare closestGridSquare = null;
		foreach (GridSquare square in gridSquares)
		{
			float newDistance = Position.DistanceTo(square.Position);
			if (distance > newDistance)
			{
				distance = newDistance;
				closestGridSquare = square;
			}
		}
		return closestGridSquare;
	}

}
