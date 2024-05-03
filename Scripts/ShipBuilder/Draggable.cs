using Godot;
using System;
using System.Collections.Generic;

public partial class Draggable : Node2D
{
	[Export]
	public ShipComponent shipComponent {get; private set;}

	private bool isInDroppable = false;

	private bool draggable = false;

	private bool ignoreNextExit = false;

	private List<GridSquare> gridSquares = new();

	private Vector2 mouseOffset;

	private Vector2 initialPosition;

    public override void _Process(double delta)
    {
		string clickActionName = "click";
        if(draggable)
		{
			if(Input.IsActionJustPressed(clickActionName))
			{
				initialPosition = GlobalPosition;
				mouseOffset = GetGlobalMousePosition() - GlobalPosition;
			}
			if(Input.IsActionPressed(clickActionName))
			{
				GlobalPosition = GetGlobalMousePosition() - mouseOffset;
			}
			else if(Input.IsActionJustReleased(clickActionName))
			{
				Tween tween = GetTree().CreateTween();
				if(isInDroppable)
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
		}
    }

    public void MouseEnter()
	{
		draggable = true;
		Scale = new Vector2(1.05f, 1.05f);
	}

	public void MouseExit()
	{
		draggable = false;
		Scale = new Vector2(1, 1);
	}

	public void EnteredSnappable(Node2D snappable)
	{
		if(isInDroppable){ignoreNextExit = true;}
		if(snappable.IsInGroup("snappable"))
		{
			isInDroppable = true;
			gridSquares.Add(snappable as GridSquare);
		}
	}

	public void ExitedSnappable(Node2D snappable)
	{
		if(gridSquares.Contains(snappable as GridSquare)) {gridSquares.Remove(snappable as GridSquare); }
		if(ignoreNextExit){return;}
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
		foreach(GridSquare square in gridSquares)
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
