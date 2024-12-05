using Godot;

public partial class Menu : Control
{
	[Export]
	private ShipComponentData[] datas;
	[Signal]
	public delegate void QuitPressedEventHandler();
	[Export]
	private Resource shipSaverClass;

	public void OnQuitPressed()
	{
		EmitSignal(SignalName.QuitPressed);
	}
}
