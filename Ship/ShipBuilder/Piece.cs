using Godot;

public class Piece
{
    public Vector2 Coordinate;
    public ShipComponentData ComponentData;
    public bool IsMirrored;
    public float LocalRotation;
    public Color Colour;

    public Piece(ShipComponentData componentData, Vector2 coordinate, bool isMirrored, float localRotation, Color colour)
    {
        ComponentData = componentData;
        Coordinate = coordinate;
        IsMirrored = isMirrored;
        LocalRotation = localRotation;
        Colour = colour;
    }
}