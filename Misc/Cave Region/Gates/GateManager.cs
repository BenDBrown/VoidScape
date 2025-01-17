using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class GateManager : Node2D
{
	private const int MAX_HEARTS_IN_SINGLE_ANIMATION = 3;

	[Export]
	private ActivateOnCondition activationCondition;

	[Export]
	private AnimatedGate[] gates;
	
	[ExportCategory("Progress Feedback Animation")]

	[Export]
	private AnimatedSprite2D[] gateHealthAnims;

	[Export]
	private AnimatedSprite2D gateChargerAnim;

	private int maxHP = 0;
	
	public override void _Ready()
	{
		activationCondition.Activate += Open;
		activationCondition.OnProgressChanged += UpdateGateFeedbackAnimation;

		ConfigureGateHealthAnim();
	}

	private void Open(){
		foreach(AnimatedGate gate in gates){
			gate.OpenGate();
		}
	}

	private void UpdateGateFeedbackAnimation(){
		int currentHealth = maxHP - activationCondition.GetAmountCompleted();

		UpdateChargeAnimation(currentHealth);
		UpdateHealthAnimation(currentHealth);
	}

	private void UpdateChargeAnimation(int currentHealth){
		if(currentHealth == 0){
			gateChargerAnim.Frame = 0; // Set animation on empty
			gateChargerAnim.Stop();
		}
		else{
			gateChargerAnim.SpeedScale = (float)currentHealth/maxHP;
		}
	}

	private void UpdateHealthAnimation(int currentHealth){
		for(int i = 0; i < gateHealthAnims.Length; i++){
			
			int spriteHealth = Mathf.Min(MAX_HEARTS_IN_SINGLE_ANIMATION, currentHealth);
			currentHealth -= spriteHealth;

			gateHealthAnims[i].Frame = 3 - spriteHealth; 
		}
	}

	private void ConfigureGateHealthAnim(){
		maxHP = activationCondition.GetAmountOfConditions();
		UpdateGateFeedbackAnimation();
	}
}
