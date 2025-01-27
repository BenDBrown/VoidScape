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
	public HUD Hud { get; set; }
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
		playerShipSaver = playerShipSaver.Call("load_save").As<Resource>();
		AddChild(Fx, true);
		ProcessMode = ProcessModeEnum.Always;
	}

	public override void _Process(double delta)
	{
		Debug();
	}

	private void Debug()
	{
		if (Input.IsKeyPressed(Key.Escape))
		{
			GetTree().ChangeSceneToFile(START_MENU_SCENE);
		}

		if (Input.IsKeyPressed(Key.F4))
		{

			playerShipSaver.Call("delete_save");
			GetTree().ChangeSceneToFile(START_MENU_SCENE);
		}
	}

	public void PlayFx(AudioStream sound, float time = 0)
	{
		Fx.Stream = sound;
		Fx.Play(time);
	}

	public bool LoadGame()
	{
		return FileAccess.FileExists((string)playerShipSaver.Call("get_location"));
	}
	public bool SaveGame() { return false; }

	public ShipBuildStatus BuildShip(Piece[] pieces)
	{
		if (PlayerShip is null) { return ShipBuildStatus.Unknown; }
		Dictionary<Vector2, ShipComponent> components = new();
		PlayerShip.Reset();
		foreach (Piece piece in pieces)
		{
			ShipComponent component = piece.ComponentData.GetPrefab();
			component.SetupData(piece.ComponentData);
			component.SetColour(piece.Colour);
			if (piece.IsMirrored) component.Mirror();
			PlayerShip.AddComponent(component, piece.Coordinate);
			component.RotationDegrees = piece.LocalRotation;
			if (component is Thruster thruster)
			{
				thruster.SetRotate();
			}

			components.Add(piece.Coordinate, component);
		}

		try
		{
			var res = PlayerShip.TryBuildShip();
			if (!res)
			{
				PlayerShip.Reset();
				playerShipSaver.Call("build_ship", PlayerShip);
				PlayerShip.TryBuildShip();
				return ShipBuildStatus.ComponentMissing;
			}
			playerShipSaver.Call("add_components", components);
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