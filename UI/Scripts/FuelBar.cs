using Godot;
using System;

[GlobalClass]
public partial class FuelBar : ProgressBar
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
            playerShip.FuelChanged -= UpdateValue;
        }
        this.playerShip = playerShip;
        playerShip.FuelChanged += UpdateValue;
    }

    private void UpdateValue(float value) => Value = value;
}
