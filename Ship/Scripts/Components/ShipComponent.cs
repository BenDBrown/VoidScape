using System;
using Godot;

[GlobalClass]
public partial class ShipComponent : CharacterBody2D
{
    [Signal]
    public delegate void OnDestroyedEventHandler(ShipComponent shipComponent);

    [Export]
    public CollisionShape2D collider { get; private set; }

    [Export]
    private Sprite2D sprite;

    [Export]
    private Sprite2D destroyedSprite;

    [Export]
    private Node healthComponent;

    [Export]
    public ShipComponentData Data;

    [Export]
    public bool IsMirrored = false;

    [Export]
    private SingleRunAnimation explosionAnim;

    private bool destroyed = false;

    public override void _Ready()
    {
        if (Data != null)
        {
            SetupData();
        }
        else
        {
            GD.PushError(GetParent().Name + "'s " + Name + " is Missing ShipComponentData");
        }
        if (healthComponent != null && healthComponent.HasSignal("died"))
        {
            healthComponent.Connect("died", Callable.From(Destroyed));
        }
    }

    public bool IsDestroyed() => destroyed;

    private void Destroyed()
    {
        if (IsDestroyed()) { return; }
        GD.PrintS(Name, " Destroyed");
        explosionAnim.Play();
        destroyed = true;
        collider.SetDeferred("disabled", true);
        sprite.Visible = !destroyed;
        destroyedSprite.Visible = destroyed;
        EmitSignal(SignalName.OnDestroyed, this);
    }

    private void Revived()
    {
        collider.SetDeferred("disabled", false);
        destroyed = false;
        sprite.Visible = !destroyed;
        destroyedSprite.Visible = destroyed;
    }

    /// <summary>
    /// Gets called in ShipComponent's _Ready.
    /// Used to set the data of a component. Sprite and Health data is pre set
    /// </summary>
    protected virtual void SetupData()
    {
        sprite.Texture = Data.Sprite;
        sprite.FlipH = IsMirrored;
        healthComponent.Call("set_component", Data.MaxHealth, Data.Defense);
        destroyedSprite.Texture = Data.DestroyedSprite;
        TopAttachable = Data.TopAttachable;
        BottomAttachable = Data.BottomAttachable;
        RightAttachable = Data.RightAttachable;
        LeftAttachable = Data.LeftAttachable;
    }

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
            destroyedSprite.FlipH = !destroyedSprite.FlipH;
        }
        else
        {
            newAttachableA = TopAttachable;
            newAttachableB = BottomAttachable;
            TopAttachable = newAttachableB;
            BottomAttachable = newAttachableA;
            sprite.FlipV = !sprite.FlipV;
            destroyedSprite.FlipV = !destroyedSprite.FlipV;
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
