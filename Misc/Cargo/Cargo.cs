using Godot;

[GlobalClass]
[Icon("res://Assets/Other/Loot Icon.png")]
public partial class Cargo : Resource
{
	[Export] Texture2D sprite;
	[Export] int sellPrice = 10;
}