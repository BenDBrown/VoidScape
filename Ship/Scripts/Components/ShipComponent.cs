using Godot;
using System;
using Godot.Collections;
using System.Linq;

[GlobalClass]
public partial class ShipComponent : Area2D
{
    [Signal]
    public delegate void OnDestroyedEventHandler(ShipComponent shipComponent);

    [Export]
    private Node2D[] vertices;

    [Export]
    private Sprite2D sprite;

    [Export]
    public bool TopAttachable { get; private set; }
    [Export]
    public bool BottomAttachable { get; private set; }
    [Export]
    public bool RightAttachable { get; private set; }
    [Export]
    public bool LeftAttachable { get; private set; }

    private bool destroyed = false;

    public bool IsDestroyed() => destroyed;

    public Vector2[] GetVertices()
    {
        Vector2[] v2Vertices = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 v2 = vertices[i].GlobalPosition;
            v2Vertices[i] = v2;
        }
        return v2Vertices;
    }

    public void Mirror()
    {
        bool newAttachableA;
        bool newAttachableB;
        if (RotationDegrees == 0 || Math.Abs(RotationDegrees) == 180)
        {
            newAttachableA = LeftAttachable;
            newAttachableB = RightAttachable;
            LeftAttachable = newAttachableB;
            RightAttachable = newAttachableA;
            sprite.FlipH = !sprite.FlipH;
        }
        else
        {
            newAttachableA = TopAttachable;
            newAttachableB = BottomAttachable;
            TopAttachable = newAttachableB;
            BottomAttachable = newAttachableA;
            sprite.FlipV = !sprite.FlipV;
        }
    }

    public void RotateRight()
    {
        bool newTop;
        bool newRight;
        bool newBottom;
        bool newLeft;
        newRight = TopAttachable;
        newBottom = RightAttachable;
        newLeft = BottomAttachable;
        newTop = LeftAttachable;
        TopAttachable = newTop;
        RightAttachable = newRight;
        BottomAttachable = newBottom;
        LeftAttachable = newLeft;
        RotationDegrees += 90;
        if (RotationDegrees >= 360) { RotationDegrees = 0; }
    }

    public void RotateLeft()
    {
        bool newTop;
        bool newLeft;
        bool newBottom;
        bool newRight;
        newRight = BottomAttachable;
        newBottom = LeftAttachable;
        newLeft = TopAttachable;
        newTop = RightAttachable;
        TopAttachable = newTop;
        RightAttachable = newRight;
        BottomAttachable = newBottom;
        LeftAttachable = newLeft;
        RotationDegrees -= 90;
        if (RotationDegrees <= -360) { RotationDegrees = 0; }
    }

    private void Destroyed()
    {
        destroyed = true;
        CollisionLayer = 0;
        CollisionMask = 0;
        Hide();
        EmitSignal(SignalName.OnDestroyed, this);
    }

    private void Revived()
    {
        CollisionLayer = 0b00000000_00000000_00000000_00000001;
        CollisionMask = 0b00000000_00000000_00000000_00000001;
        destroyed = false;
        Visible = true;
    }
}
