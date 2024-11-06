using Godot;

public class NewBuilder
{
    private const int SPRITE_SIZE = 32;
    [Export]
    private PackedScene PlayerShipScene;
    private Resource saveable;
    public void Build(Piece[] pieces, Vector2 spawnPosition)
    {
        Ship player = PlayerShipScene.Instantiate() as Ship;
        saveable.Call("clear");
        foreach (var piece in pieces)
        {
            ShipComponent component = piece.ComponentData.GetPrefab();
            component.Data = piece.ComponentData;
            player.AddChild(component);
            component.IsMirrored = piece.IsMirrored;
            component.Rotation = piece.LocalRotation;
            component.Position = piece.Coordinate * SPRITE_SIZE;
            saveable.Call("add_component", piece.Coordinate, component);
        }
        player.TryBuildShip();
        player.GlobalPosition = spawnPosition;
        Game.Instance.PlayerShip = player;
    }
}
