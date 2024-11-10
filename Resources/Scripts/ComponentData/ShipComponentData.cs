using Godot;

[GlobalClass]
public partial class ShipComponentData : Resource
{
	[Export]
	public string Name = "Component";
	[Export]
	public string Description = "Hello";

	[ExportCategory("Visual & UI")]
	[Export]
	public Texture2D Sprite { get; private set; }

	[Export]
	public bool TopAttachable { get; private set; }
	[Export]
	public bool BottomAttachable { get; private set; }
	[Export]
	public bool LeftAttachable { get; private set; }
	[Export]
	public bool RightAttachable { get; private set; }
	[Export]
	public ComponentType Type = ComponentType.COMPONENT;

	[ExportCategory("Ship Data")]
	[Export]
	public int MaxHealth = 100;
	[Export]
	public int Defense = 10;
	[Export]
	private PackedScene prefab;

	public ShipComponent GetPrefab() => prefab.Instantiate() as ShipComponent;
	public string GetPrefabPath() => prefab.ResourcePath;
}