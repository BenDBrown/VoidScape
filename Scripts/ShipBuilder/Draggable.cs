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

	private Vector2 startPos;

	private string clickActionName = "click";

	private const string rotateRightActionName = "rotate_part_right";

	private const string rotateLeftActionName = "rotate_part_left";

	private const string mirrorActionName = "mirror_part";

    public override void _Ready() => startPos = GlobalPosition;

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
			if(Input.IsActionJustPressed(rotateRightActionName)) { shipComponent.RotateRight(); }
			else if(Input.IsActionJustPressed(rotateLeftActionName)) { shipComponent.RotateLeft(); }
			else if(Input.IsActionJustPressed(mirrorActionName)) { shipComponent.Mirror(); }
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
		
		if (isInDroppable)
		{
			GridSquare gridSquare = GetNearestGridSquare();
			if(gridSquare.TrySetComponent(shipComponent, this))
			{
				Tween tween = GetTree().CreateTween();
				tween.TweenProperty(this, "position", gridSquare.Position, 0.2f).SetEase(Tween.EaseType.Out);
			}
			else { ReturnToOriginalPosition(); }
		}
		else
		{
			Tween tween = GetTree().CreateTween();
			tween.TweenProperty(this, "global_position", startPos, 0.2f).SetEase(Tween.EaseType.Out);
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
		if (square.shipComponent == shipComponent) { square.PullComponent(); }
		if (gridSquares.Count < 1) { isInDroppable = false; }
	}

	public void SetShipComponent(ShipComponent shipComponent)
	{
		AddChild(shipComponent);
		shipComponent.Position = Position;
		this.shipComponent = shipComponent;
	}

	public void ReturnToOriginalPosition() => GlobalPosition = startPos;

	public void SetStartPosition(Vector2 startPos) => this.startPos = startPos;

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
