using Godot;
using System;

public partial class Crystal : Area2D
{
    [Signal]
    public delegate void CrystalHitEventHandler(Crystal sender);

    [Export]
    private ColourableSprite sprite;

    [Export]
    public Crystal[] ConnectedCrystals;

    [Export]
    private Color inactiveCol;

    [Export]
    private Color activeCol;

    public bool Active {get; private set;} = false;

    public void OnBodyEntered(Node2D body)
    {
        EmitSignal(SignalName.CrystalHit, this);
    }

    public void Flip()
    {
        Active = !Active;
        if(Active) sprite.SetColour(activeCol);
        else sprite.SetColour(inactiveCol);
    }
}
