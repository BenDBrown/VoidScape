using Godot;
using System;

public partial class PopUpMsg : CanvasLayer
{

	[Export]
	public string text;

	[Export]
	public Color color;

	[Export]
	public Label label;

	[Export]
	public ColorRect background;


	public override void _Ready()
	{
		CallDeferred("Init");
	}

	public void Init()
	{
		background.Color = color;
		label.Text = text;
	}
}
