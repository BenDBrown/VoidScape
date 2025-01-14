using Godot;
using System;

public partial class OpenGate : Node2D
{

	[Export]
	private ActivateOnCondition activationCondition;

	[Export]
	private AnimationPlayer animation;
	
	[ExportCategory("Progress Feedback Animation")]
	[Export]
	private AnimatedSprite2D gateHealthAnim;

	[Export]
	private AnimatedSprite2D gateChargerAnim;

	private int gateHealthFrame = 0;
	private double chargeSpeedScale = 1;
	
	public override void _Ready()
	{
		activationCondition.Activate += Open;
		activationCondition.OnProgressChanged += UpdateGateFeedbackAnimation;
	}

	private void Open(){
		animation.Play("Open");
	}

	private void UpdateGateFeedbackAnimation(){
		if(gateHealthFrame < 3){

			// Update health anim
			gateHealthFrame++;
			gateHealthAnim.Frame = gateHealthFrame;
			
			// Updating charge anim
			if(gateHealthFrame == 3){
				chargeSpeedScale = 0;
				gateChargerAnim.Frame = 0;
			}
			else{
				chargeSpeedScale= chargeSpeedScale - 0.3;
			}
			
			gateChargerAnim.SpeedScale = (float)chargeSpeedScale;
			GD.Print(gateChargerAnim.SpeedScale);
		}
	}

}
