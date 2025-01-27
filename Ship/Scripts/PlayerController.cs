using Godot;
using System;

public partial class PlayerController : Node
{

    private const int CYCLE_WEAPON_UP = 1;
    private const int CYCLE_WEAPON_DOWN = -1;

    [Export]
    private Node2D playerShipNode;

    private PlayerShip playerShip;
    private bool canBlink = false;
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
        if (Input.IsActionJustPressed("boosting")) { canBlink = true; }

        if (Input.IsActionJustPressed("shoot"))
        {
            playerShip.StartShooting();
            playerShip.StopShielding();
        }
        else if (Input.IsActionJustReleased("shoot")) { playerShip.StopShooting(); }

        if (Input.IsActionJustPressed("rotate_right")) { playerShip.StartTurningClockwise(); }
        else if (Input.IsActionJustPressed("rotate_left")) { playerShip.StartTurningCounterClockwise(); }
        else if ((Input.IsActionJustReleased("rotate_right") && (!Input.IsActionPressed("rotate_left"))) || (Input.IsActionJustReleased("rotate_left") && (!Input.IsActionPressed("rotate_right")))) { playerShip.StopTurning(); }


        // Weapon Menu Controls
        if (Input.IsActionJustPressed("toggle_weapon_menu"))
        {        // Change name of action to toggle weapon menu
            playerShip.ToggleWeaponMenu(true);
        }
        else if (Input.IsActionJustReleased("toggle_weapon_menu"))
        {
            playerShip.ToggleWeaponMenu(false);
        }

        if (Input.IsActionJustPressed("cycle_weapon_up"))
        {
            playerShip.CycleGunGroup(CYCLE_WEAPON_UP);
        }
        else if (Input.IsActionJustPressed("cycle_weapon_down"))
        {
            playerShip.CycleGunGroup(CYCLE_WEAPON_DOWN);
        }

        //interact controls
        if (Input.IsActionJustPressed("interact")) playerShip.Interact();

        if (Input.IsActionJustPressed("shielding") && (!Input.IsActionPressed("shoot"))) { playerShip.StartShielding(); }
        if (Input.IsActionJustReleased("shielding")) { playerShip.StopShielding(); }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (canBlink)
        {
            playerShip.PerformBlink(GetPlayerInputDirection());
            canBlink = false;
        }
    }
    public Vector2 GetPlayerInputDirection()
    {
        Vector2 returnVect = Vector2.Zero;
        if (Input.IsActionPressed("forward")) returnVect += Vector2.Up;
        else if (Input.IsActionPressed("back")) returnVect += Vector2.Down;
        if (Input.IsActionPressed("rotate_right")) returnVect += Vector2.Right;
        else if (Input.IsActionPressed("rotate_left")) returnVect += Vector2.Left;
        return returnVect.Normalized();
    }
}
