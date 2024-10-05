using Godot;
using System;

[GlobalClass]
public partial class GridSquare : StaticBody2D
{
	private const int COORDINATE_RATIO = 32;
    public delegate void ShipComponentPlacedEventHandler(object sender);

	public event ShipComponentPlacedEventHandler ShipComponentPlaced;

	[Export]
	private Sprite2D validSprite;

	[Export]
	private Sprite2D invalidSprite;

	public ShipComponent shipComponent { get; private set; }
	public Draggable draggable{ get; private set; }
	public Vector2 Coordinate { get; set; }
	public Vector2 CoordinateToPosition() => new Vector2(Coordinate.X * COORDINATE_RATIO, Coordinate.Y * COORDINATE_RATIO);

	public bool IsValid {get; private set;} = true;

	public void SetValidity(bool valid) 
	{
		IsValid = valid;
		validSprite.Visible = valid;
		invalidSprite.Visible = !valid;
	}

	public bool TrySetComponent(ShipComponent shipComponent, Draggable draggable)
	{
		if (!IsValid) {return IsValid; }
		if(this.draggable != null) { this.draggable.ReturnToOriginalPosition(); }
		this.draggable = draggable;
		this.shipComponent = shipComponent;
		ShipComponentPlaced?.Invoke(this);
		return IsValid;
	}

	public void PullComponent() // to be used when the user drags a component out
	{
		draggable = null;
		shipComponent = null;
	}

	public void RemoveComponent() // to be used when code causes a part to be returned for instance because the part connecting it to the ship was removed
	{
		if(draggable != null) { draggable.ReturnToOriginalPosition(); }
		PullComponent();
	}
}