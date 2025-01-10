using Godot;
using System;

[GlobalClass]
public partial class CreditsPouch : Label
{
	public override void _Ready()
	{
		ConfigurePouch(Game.Instance.PlayerShip);
	}

    public void ConfigurePouch(PlayerShip playerShip)
    {
        if(playerShip != null) 
        {
            playerShip.CreditsChanged -= UpdateCreditsDisplay;
        }
        playerShip.CreditsChanged += UpdateCreditsDisplay;
    }

	public void UpdateCreditsDisplay(float totalCredits){
		Text = $"{totalCredits}";
	}
}
