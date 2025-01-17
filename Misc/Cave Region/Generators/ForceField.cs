using Godot;
using System;

public partial class ForceField : Node2D
{
	private const string COLOR_SHADER_PARAMETER = "color";
	private const float MIN_FADE_OPACITY = 0.0f;

	[Export]
	private Sprite2D sprite;
	[Export]
	private double fadeDuration = 1.0f;

	[Export(PropertyHint.Range, "0,1,")]
	private float maxFadeOpacity = 1.0f;

	private ShaderMaterial shaderMaterial;
	private Color mainColor;
	private double elapsedTime = 0;
	private bool isfading = false;

	public override void _Ready()
	{
		shaderMaterial = sprite.Material as ShaderMaterial;

		// Set Starting Opacity to zero
		Color currentColor = (Color) shaderMaterial.GetShaderParameter(COLOR_SHADER_PARAMETER);
		currentColor.A = 0;
		shaderMaterial.SetShaderParameter(COLOR_SHADER_PARAMETER, currentColor);
	}

	public override void _Process(double delta)
	{
		if(isfading){
			FadeShield(delta);
		}
	}

	public void OnAreaEntered(Area2D area){
		StartFade();
	}

	public void OnBodyEntered(Node2D body){
		StartFade();
	}

	public void StartFade(){
		UpdateOpacity(maxFadeOpacity);
		elapsedTime = 0.0f;
		isfading = true;
	}

	public void DestroyForcefield(){
		QueueFree();
	}

	private void FadeShield(double delta){
		elapsedTime += delta;

		double fadeProgress = Mathf.Clamp(elapsedTime/fadeDuration, MIN_FADE_OPACITY, maxFadeOpacity);
		double newOpacity = Mathf.Lerp(maxFadeOpacity, MIN_FADE_OPACITY, fadeProgress);
		UpdateOpacity((float)newOpacity);

		if(fadeProgress >= maxFadeOpacity){
			isfading = false;
			UpdateOpacity(0);
		}
	}

	private void UpdateOpacity(float opacity){
		Color currentColor = (Color) shaderMaterial.GetShaderParameter(COLOR_SHADER_PARAMETER);
		currentColor.A = opacity;
		shaderMaterial.SetShaderParameter(COLOR_SHADER_PARAMETER, currentColor);
	}
}
