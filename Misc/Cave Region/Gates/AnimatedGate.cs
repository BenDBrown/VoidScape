using Godot;
using System;

public partial class AnimatedGate : Node2D
{
	[Export]
	private string animationName = "Open";

	[Export]
	private AnimationPlayer animation;

	private bool isOpen = false;

    public override void _Ready()
    {
        animation.CurrentAnimation = animationName;
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
