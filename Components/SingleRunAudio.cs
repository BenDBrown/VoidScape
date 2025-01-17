using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;

[GlobalClass]
public partial class SingleRunAudio : AudioStreamPlayer2D
{
	[Export]
	private bool shouldShiftPitch = false;

	[Export(PropertyHint.Range, "0,0.5")]
	private float pitchRange = 0.5f;	
	public float fromPosition { get; set;}

	public void PlayOnce(){
		if(shouldShiftPitch){
			ShiftPitch();
		}

		this.Play(fromPosition);
	}

	private void ShiftPitch(){
		this.PitchScale += (float)GD.RandRange((double)-pitchRange, (double)pitchRange);
	}
}
