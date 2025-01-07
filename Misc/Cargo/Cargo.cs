using Godot;

[GlobalClass]
[Icon("res://Assets/Other/Loot Icon.png")]
public partial class Cargo : Resource
{
	[Export] Texture2D sprite;
	private PackedScene lootScene = GD.Load("res://Misc/Cargo/loot_drop.tscn") as PackedScene;

	public void Spawn(Node currentScene, Vector2 globalPosition)
	{
		Node2D lootDrop = lootScene.Instantiate() as Node2D;
		currentScene.AddChild(lootDrop);
		lootDrop.Call("set_sprite", sprite);
		lootDrop.Set("cargo", this);
		lootDrop.GlobalPosition = globalPosition;
	}
}