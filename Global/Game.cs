using System;
using Desktop.Ship.Scripts;
using Godot;
using Godot.Collections;

public partial class Game : Node
{
	private const string START_MENU_SCENE = "res://Scenes/start_menu.tscn";
	public static Game Instance { get; set; }
	public string SAVE_PATH = "res://saves/";
	public Ship PlayerShip { get; set; }
	public AudioStreamPlayer2D Fx;
	private const string SHIP_SAVER_PATH = "res://Resources/Scripts/player_ship_save.gd";
	[Export]
	private Resource saver;

	public Game()
	{
		if (Instance != null) { QueueFree(); return; }

		Instance = this;
		Fx = new();
		AddChild(Fx);
	}

	public override void _Process(double delta)
	{
		Debug();
	}

	private void Debug()
	{
		if (!OS.HasFeature("debug")) { return; }
		if (Input.IsKeyPressed(Key.F4)) { GetTree().Quit(); }
		if (Input.IsKeyPressed(Key.Escape)) { GetTree().ChangeSceneToFile(START_MENU_SCENE); }
	}

	public void PlayFx(AudioStream sound, float time = 0)
	{
		Fx.Stream = sound;
		Fx.Play(time);
	}

	public bool LoadGame() { return false; }
	public bool SaveGame() { return false; }
	public ShipBuildStatus BuildShip(Piece[] pieces)
	{
		var saver = GD.Load(SHIP_SAVER_PATH).Call("new").As<Resource>();

		GD.Print(saver);
		Dictionary<Vector2, ShipComponent> comps = new();
		Node2D parent = new();
		AddChild(parent);
		foreach (Piece piece in pieces)
		{
			ShipComponent component = piece.ComponentData.GetPrefab();
			component.Data = piece.ComponentData;
			component.IsMirrored = piece.IsMirrored;
			component.Rotation = piece.LocalRotation;
			parent.AddChild(component);
			comps.Add(piece.Coordinate, component);
		}
		try
		{
			saver.Call("add_components", comps);
			saver.Call("save");
		}
		catch (Exception e)
		{
			GD.Print(e);
			GD.PushError(e);
		}
		return ShipBuildStatus.OK;
	}
}