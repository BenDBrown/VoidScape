using Godot;
using System;

public partial class Component : Control
{
	[Export]
	public ShipComponentData DataResource;

	[Export]
	public TextureRect texture;
	
	public override void _Ready()
	{
		texture.Texture = DataResource.Sprite;

	}

	
	public override void _Process(double delta)
	{
	}
}
