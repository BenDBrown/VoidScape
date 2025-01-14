using Godot;
using System;

public partial class PlayerController : Node
{

    private const int CYCLE_WEAPON_UP = 1;
    private const int CYCLE_WEAPON_DOWN = -1;

    [Export]
    private Node2D playerShipNode;

    private PlayerShip playerShip;

    public override void _Ready()
    {
        if (playerShipNode is PlayerShip) { playerShip = playerShipNode as PlayerShip; }
        else { GD.PrintErr("player ship node was not an IShip"); }
    }

    public override void _Process(double delta)
    {
        if (Game.Instance.IsUiOpen) { return; }
        if (Input.IsActionJustPressed("forward")) { playerShip.StartThrustingForward(); }
        else if (Input.IsActionJustPressed("back")) { playerShip.StartThrustingBackward(); }
        if (Input.IsActionJustPressed("right")) { playerShip.StartThrustingRight(); }
        else if (Input.IsActionJustPressed("left")) { playerShip.StartThrustingLeft(); }
        if (Input.IsActionJustReleased("forward")) { playerShip.StopThrustingForward(); }
        if (Input.IsActionJustReleased("back")) { playerShip.StopThrustingBackward(); }
        if (Input.IsActionJustReleased("right")) { playerShip.StopThrustingRight(); }
        if (Input.IsActionJustReleased("left")) { playerShip.StopThrustingLeft(); }
        if (Input.IsActionJustPressed("boosting")){playerShip.PerformBlink( );}

        if (Input.IsActionJustPressed("shoot"))
        {
            playerShip.StartShooting();
            playerShip.StopShielding();
        }
        else if (Input.IsActionJustReleased("shoot")) { playerShip.StopShooting(); }

        if(Input.IsActionJustPressed("rotate_right")) { playerShip.StartTurningClockwise(); }
        else if(Input.IsActionJustPressed("rotate_left")) { playerShip.StartTurningCounterClockwise(); }
        else if((Input.IsActionJustReleased("rotate_right") && (!Input.IsActionPressed("rotate_left"))) || (Input.IsActionJustReleased("rotate_left") && (!Input.IsActionPressed("rotate_right")))) { playerShip.StopTurning(); }
    

        // Weapon Menu Controls
        if(Input.IsActionJustPressed("toggle_weapon_menu")){        // Change name of action to toggle weapon menu
            ((PlayerShip) playerShip).ToggleWeaponMenu(true);
        }
        else if( Input.IsActionJustReleased("toggle_weapon_menu")){
            ((PlayerShip) playerShip).ToggleWeaponMenu(false);
        }

        if(Input.IsActionJustPressed("cycle_weapon_up")){
            ((PlayerShip) playerShip).CycleGunGroup(CYCLE_WEAPON_UP);
        }
        else if(Input.IsActionJustPressed("cycle_weapon_down")){
            ((PlayerShip) playerShip).CycleGunGroup(CYCLE_WEAPON_DOWN);
        }

        //interact controls
        if(Input.IsActionJustPressed("interact")){
            playerShip.Interact();
        }

        if (Input.IsActionJustPressed("shielding") && (!Input.IsActionPressed("shoot"))) { playerShip.StartShielding(); }
        if (Input.IsActionJustReleased("shielding")) { playerShip.StopShielding(); }
    }
}
