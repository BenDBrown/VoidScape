using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class ShipComponentData : Resource
{
	[Export]
	public string Description = "Hello";
	[Export]
	public Texture2D Sprite;
	[Export]
	public bool TopAttachable;
	[Export]
	public bool BottomAttachable;
	[Export]
	public bool LeftAttachable;
	[Export]
	public bool RightAttachable;
	[Export]
	public ComponentType Type = ComponentType.COMPONENT;

	[ExportCategory("Health Component")]

	[Export]
	public int MaxHealth = 100;

	[Export]
	public int Defense = 10;
}
