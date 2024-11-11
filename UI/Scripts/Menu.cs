using Godot;

public partial class Menu : Control
{
	[Export]
	private ShipComponentData[] datas;
	[Signal]
	public delegate void QuitPressedEventHandler();
	[Export]
	private Resource shipSaverClass;

	public void OnBuildPressed() //TODO: Change this to retrieve info from the build area
	{
		Piece[] pieces = new Piece[datas.Length];
		for (int i = 0; i < datas.Length; i++)
		{
			Piece piece = new(datas[i], new Vector2(0, i), false, 0);
			pieces[i] = piece;
			GD.Print(piece.ComponentData.Name);
		}
		Game.Instance.BuildShip(pieces);
	}

	public void OnQuitPressed()
	{
		EmitSignal(SignalName.QuitPressed);
	}
}
