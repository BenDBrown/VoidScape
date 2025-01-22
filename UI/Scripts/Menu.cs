using Godot;

public partial class Menu : Control
{
	[Signal]
	public delegate void OnReadyFinishedEventHandler();
	[Signal]
	public delegate void QuitPressedEventHandler();
	[Export]
	private Resource shipSaverClass;
	[Export]
	public Station stationHub;
	public ShipComponentData[] Datas { get; private set; }

	public override void _Ready()
	{
		GD.Print("Items: " + stationHub.Datas.Length);
		Datas = stationHub.Datas;
		EmitSignal(SignalName.OnReadyFinished);
	}

	public void OnQuitPressed()
	{
		EmitSignal(SignalName.QuitPressed);
	}
}
