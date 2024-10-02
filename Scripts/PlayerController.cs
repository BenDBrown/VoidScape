using Godot;
using System;

public partial class PlayerController : Node
{
    [Export]
    private Node2D playerShipNode;

    private IShip playerShip;

    public override void _Ready()
    {
        if(playerShipNode is IShip){playerShip = playerShipNode as IShip;}
        else {GD.PrintErr("player ship node was not an IShip");}
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("forward")) { playerShip.ForwardThrust(); }
        else if(Input.IsActionJustPressed("back")) { playerShip.BackThrust(); }
        else if((Input.IsActionJustReleased("forward") && (!Input.IsActionPressed("back"))) || (Input.IsActionJustReleased("back") && (!Input.IsActionPressed("forward")))) { playerShip.StopThrusting(); }

        if(Input.IsActionJustPressed("shoot")) { playerShip.StartShooting(); }
        else if(Input.IsActionJustReleased("shoot")) { playerShip.StopShooting(); }

        if(Input.IsActionJustPressed("rotate_right")) { playerShip.StartTurningClockwise(); }
        else if(Input.IsActionJustPressed("rotate_left")) { playerShip.StartTurningCounterClockwise(); }
        else if((Input.IsActionJustReleased("rotate_right") && (!Input.IsActionPressed("rotate_left"))) || (Input.IsActionJustReleased("rotate_left") && (!Input.IsActionPressed("rotate_right")))) { playerShip.StopTurning(); }
    }
}
