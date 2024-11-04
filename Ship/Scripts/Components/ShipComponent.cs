using System;
using Godot;

[GlobalClass]
public partial class ShipComponent : CharacterBody2D
{
    [Signal]
    public delegate void OnDestroyedEventHandler(ShipComponent shipComponent);

    [Export]
    private CollisionShape2D collider;

    [Export]
    private Sprite2D sprite;

    [Export]
    private Node healthComponent;

    [Export]
    public ShipComponentData Data;

    private bool destroyed = false;

    public override void _Ready()
    {
        if (Data != null)
        {
            Data.SetUp(this);
        }
        if (healthComponent != null && healthComponent.HasSignal("died"))
        {
            healthComponent.Connect("died", Callable.From(Destroyed));
        }
    }

    public bool IsDestroyed() => destroyed;

    private void Destroyed()
    {
        if (destroyed) { return; }
        GD.PrintS(Name, " Destroyed");
        destroyed = true;
        collider.SetDeferred("disabled", true);
        Hide();
        EmitSignal(SignalName.OnDestroyed, this);
    }

    private void Revived()
    {
        collider.SetDeferred("disabled", false);
        Show();
        destroyed = false;
        Visible = true;
    }

    public void SetSprite(Texture2D texture) => sprite.Texture = texture;

    public void SetHealthComponent(int maxHealth, int defense) => healthComponent.Call("set_component", maxHealth, defense);


    #region ToBeRemoved
    public bool TopAttachable { get; private set; }
    public bool BottomAttachable { get; private set; }
    public bool RightAttachable { get; private set; }
    public bool LeftAttachable { get; private set; }
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
    #endregion
}
