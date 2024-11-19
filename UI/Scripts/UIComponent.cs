using Godot;
using System;

public partial class UIComponent : Control
{
	[Export]
	public ShipComponentData DataResource;

	[Export]
	public TextureRect texture;

	public override void _Ready()
	{
		texture.Texture = DataResource.Sprite;
	}

}
