using Godot;
using System;

public partial class AnimatedGate : Node2D
{
	private const string OPEN_GATE_ANIMATION = "Open";

	[Export]
	private AnimationPlayer animation;

	private bool isOpen = false;

    public override void _Ready()
    {
        animation.CurrentAnimation = OPEN_GATE_ANIMATION;
		animation.Pause();
    }

    public void OpenGate(){
		animation.Play();
	}

	public void CloseGate(){
		animation.PlayBackwards();
	}

	public void PauseGate(){
		animation.Pause();
	}

}
