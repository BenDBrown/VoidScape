using Godot;

public class Piece
{
    public Vector2 Coordinate;
    public ShipComponentData ComponentData;
    public bool IsMirrored;
    public float LocalRotation;
    public Color Colour;

    public Piece(ShipComponent shipComponent, Vector2I coordinate) : this(shipComponent.Data, (Vector2)coordinate, shipComponent.IsMirrored, Mathf.Round(shipComponent.RotationDegrees))
    {
    }


    public Piece(ShipComponentData componentData, Vector2 coordinate, bool isMirrored, float localRotation)
    {
        ComponentData = componentData;
        Coordinate = coordinate;
        IsMirrored = isMirrored;
        LocalRotation = localRotation;
    }


}