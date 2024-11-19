using Godot;
using System;

[GlobalClass]
public partial class SingleRunAnimation : AnimatedSprite2D
{
	[Export]
	private bool GoInvisibleOnEnd = true;

	[Export]
	private string animationName;

	public override void _Ready()
	{
		AnimationFinished += StopPlaying;
	}

	public void Play()
	{
		Visible = true;
		Play(animationName);
	}
	private void StopPlaying() => Visible = !GoInvisibleOnEnd;

}
