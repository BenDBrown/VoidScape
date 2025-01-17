using System;
using System.Threading.Tasks;
using Desktop.Ship.Scripts;
using Godot;
using Godot.Collections;

public partial class Game : Node
{
	private const string START_MENU_SCENE = "res://Scenes/start_menu.tscn";
	public static Game Instance { get; set; }
	public string SAVE_PATH = "res://saves/";
	public PlayerShip PlayerShip { get; set; }
	public CanvasLayer Hud { get; set; }
	public AudioStreamPlayer2D Fx;
	private const string SHIP_SAVER_PATH = "res://Resources/Scripts/player_ship_save.gd";
	private Resource playerShipSaver;
	public bool IsUiOpen = false;
	public Game()
	{
		if (Instance != null) { QueueFree(); return; }

		Instance = this;
		Fx = new();
		playerShipSaver = GD.Load(SHIP_SAVER_PATH).Call("new").As<Resource>();
		AddChild(Fx);
		ProcessMode = ProcessModeEnum.Always;
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
		if (PlayerShip is null) { return ShipBuildStatus.Unknown; }
		Dictionary<Vector2, ShipComponent> comps = new();
		PlayerShip.Reset();
		foreach (Piece piece in pieces)
		{
			ShipComponent component = piece.ComponentData.GetPrefab();
			component.SetupData(piece.ComponentData);
			component.SetColour(piece.Colour);
			component.IsMirrored = piece.IsMirrored;
			PlayerShip.AddComponent(component, piece.Coordinate);
			component.RotationDegrees = piece.LocalRotation;
			comps.Add(piece.Coordinate, component);
		}

		try
		{
			PlayerShip.TryBuildShip();
			playerShipSaver.Call("add_components", comps);
			playerShipSaver.Call("save");
		}
		catch (Exception e)
		{
			GD.Print(e);
			GD.PushError(e);
		}

		return ShipBuildStatus.OK;
	}
}