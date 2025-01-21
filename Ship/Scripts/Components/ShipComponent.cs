using System;
using Godot;

[GlobalClass]
public partial class ShipComponent : CharacterBody2D
{
    [Signal]
    public delegate void OnDestroyedEventHandler(ShipComponent shipComponent);

    [Export]
    public CollisionShape2D collider { get; private set; }

    public ShipComponentData Data => data;

    public Color Colour => sprite.Colour;

    [Export]
    private ColourableSprite sprite;

    [Export]
    private ColourableSprite destroyedSprite;

    [Export]
    private Node healthComponent;

    [Export]
    protected ShipComponentData data;

    [Export]
    public bool IsMirrored = false;

    [Export]
    private SingleRunAnimation explosionAnim;

    private bool destroyed = false;

    public override void _Ready()
    {
        if(data != null) SetupData(data);
        if (healthComponent != null && healthComponent.HasSignal("died"))
        {
            healthComponent.Connect("died", Callable.From(Destroyed));
        }
    }

    public bool IsDestroyed() => destroyed;

    public void SetColour(Color colour)
    {
        sprite.SetColour(colour);
        destroyedSprite.SetColour(colour);
    }

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
    public virtual void SetupData(ShipComponentData data)
    {
        if(data == null) GD.PushError(GetParent().Name + "'s " + Name + " is Missing ShipComponentData");
        this.data = data;
        sprite.Texture = this.data.Sprite;
        sprite.FlipH = IsMirrored;
        healthComponent.Call("set_component", this.data.MaxHealth, this.data.Defense);
        destroyedSprite.Texture = this.data.DestroyedSprite;
        destroyedSprite.FlipH = IsMirrored;
        TopAttachable = this.data.TopAttachable;
        BottomAttachable = this.data.BottomAttachable;
        RightAttachable = this.data.RightAttachable;
        LeftAttachable = this.data.LeftAttachable;
    }

    #region ToBeRemoved
    public bool TopAttachable { get; private set; }
    public bool BottomAttachable { get; private set; }
    public bool RightAttachable { get; private set; }
    public bool LeftAttachable { get; private set; }
    public void Mirror()
    {
        IsMirrored = !IsMirrored;
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
