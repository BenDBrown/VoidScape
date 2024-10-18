using Godot;
using System;

public partial class ShipParts : ItemList
{
	[Export]
	public PackedScene[] shipComponents;

	public override void _Ready()
	{
		Set("theme_override_constants/v_separation", 15);
		Clear();

		for (int i = 0; i < shipComponents.Length; i++)
		{
			PackedScene itemScene = shipComponents[i];
			if (itemScene != null)
			{
				Node instance = itemScene.Instantiate();
				Sprite2D sprite = (Sprite2D)instance.FindChild("Sprite2D");

				AddItem(instance.Name, sprite.Texture);
			}
		}


	}
}
