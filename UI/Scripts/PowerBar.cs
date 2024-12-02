using Godot;
using System;

[GlobalClass]
public partial class PowerBar : ProgressBar
{
    private PlayerShip playerShip;

    public override void _Ready()
    {
        base._Ready();
        SetPlayerShip(Game.Instance.PlayerShip);
    }

    public void SetPlayerShip(PlayerShip playerShip)
    {
        if(this.playerShip != null) 
        {
            playerShip.PowerChanged -= UpdateValue;
        }
        this.playerShip = playerShip;
        playerShip.PowerChanged += UpdateValue;
    }

    private void UpdateValue(float value) => Value = value;
}
