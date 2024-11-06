using Godot;

public partial class Game : Node
{
	public static Game Instance { get; set; }
	public string SAVE_PATH = "res://saves/";
	private const string START_MENU_SCENE = "res://Scenes/start_menu.tscn";
	public Ship PlayerShip { get; set; }
	public AudioStreamPlayer2D Fx;


	public Game()
	{
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
	public void SaveShip()
	{

	}
}